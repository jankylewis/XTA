using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using XTAReportingEngine.Events;

namespace XTAReportingEngine;

internal static class Program
{
    private static readonly JsonSerializerOptions s_jsonOpts = new(JsonSerializerOptions.Default)
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = false
    };

    private static string ReportRoot => Path.Combine(AppContext.BaseDirectory, "report");

    public static async Task Main(string[] args)
    {
        Console.WriteLine("XTA Reporting Engine starting...");
        Directory.CreateDirectory(ReportRoot);

        var factory = new ConnectionFactory
        {
            HostName = Environment.GetEnvironmentVariable("XTA_RMQ_HOST") ?? "localhost",
            AutomaticRecoveryEnabled = true
        };

        await using var connection = await factory.CreateConnectionAsync();
        await using var channel = await connection.CreateChannelAsync();

        const string exchange = "xta.test.events";
        const string queue = "xta.report.events";

        await channel.ExchangeDeclareAsync(exchange, ExchangeType.Direct, durable: true);
        await channel.QueueDeclareAsync(queue, durable: true, exclusive: false, autoDelete: false);
        await channel.QueueBindAsync(queue, exchange, routingKey: "#");
        await channel.BasicQosAsync(0, 50, false);

        var consumer = new AsyncEventingBasicConsumer(channel);
        consumer.ReceivedAsync += async (_, ea) =>
        {
            try
            {
                var json = Encoding.UTF8.GetString(ea.Body.ToArray());
                var root = JsonDocument.Parse(json).RootElement;
                var eventType = root.GetProperty("eventType").GetString();

                switch (eventType)
                {
                    case nameof(XTAEventTypes.RunStarted):
                        await HandleRunStartedAsync(root);
                        break;
                    case nameof(XTAEventTypes.RunCompleted):
                        await HandleRunCompletedAsync(root);
                        break;
                    case nameof(XTAEventTypes.CaseStarted):
                        await HandleCaseStartedAsync(root);
                        break;
                    case nameof(XTAEventTypes.CaseCompleted):
                        await HandleCaseCompletedAsync(root);
                        break;
                    case nameof(XTAEventTypes.StepLogged):
                        await HandleStepLoggedAsync(root);
                        break;
                    case nameof(XTAEventTypes.LogWritten):
                        await HandleLogWrittenAsync(root);
                        break;
                    case nameof(XTAEventTypes.AttachmentAdded):
                        await HandleAttachmentAddedAsync(root);
                        break;
                    default:
                        Console.WriteLine($"Unknown eventType: {eventType}");
                        break;
                }

                await channel.BasicAckAsync(ea.DeliveryTag, multiple: false);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error processing message: {ex.Message}");
                await channel.BasicNackAsync(ea.DeliveryTag, multiple: false, requeue: true);
            }
        };

        // Start consuming and keep the process alive. The consumer uses an event-driven callback; no explicit loop is needed.
        var consumerTag = await channel.BasicConsumeAsync(queue, autoAck: false, consumer);

        Console.CancelKeyPress += async (_, e) =>
        {
            e.Cancel = true;
            Console.WriteLine("Shutting down...");
            await channel.BasicCancelAsync(consumerTag);
        };

        await Task.Delay(Timeout.InfiniteTimeSpan);
    }

    private static async Task HandleRunStartedAsync(JsonElement root)
    {
        var run = new
        {
            runID = root.GetProperty("runID").GetString(),
            runName = root.TryGetProperty("runName", out var rn) ? rn.GetString() : null,
            machine = root.TryGetProperty("machine", out var mc) ? mc.GetString() : Environment.MachineName,
            branch = root.TryGetProperty("branch", out var br) ? br.GetString() : null,
            commit = root.TryGetProperty("commit", out var cm) ? cm.GetString() : null,
            startedUTC = root.GetProperty("timestampUTC").GetDateTime(),
            endedUTC = (DateTime?)null,
            totals = new { passed = 0, failed = 0, skipped = 0 }
        };

        await WriteJsonAsync(Path.Combine(ReportRoot, "run.json"), run);
        await WriteJsonAsync(Path.Combine(ReportRoot, "index.json"), new
        {
            runID = run.runID,
            totals = run.totals,
            durationMs = 0,
            cases = Array.Empty<object>()
        });
    }

    private static async Task HandleRunCompletedAsync(JsonElement root)
    {
        var runFile = Path.Combine(ReportRoot, "run.json");
        if (!File.Exists(runFile)) return;
        using var doc = JsonDocument.Parse(await File.ReadAllTextAsync(runFile));
        var runObj = doc.RootElement;
        var startedUtc = runObj.GetProperty("startedUTC").GetDateTime();
        var endedUtc = root.GetProperty("timestampUTC").GetDateTime();

        var updated = new
        {
            runID = runObj.GetProperty("runID").GetString(),
            runName = runObj.TryGetProperty("runName", out var rn) ? rn.GetString() : null,
            machine = runObj.TryGetProperty("machine", out var mc) ? mc.GetString() : Environment.MachineName,
            branch = runObj.TryGetProperty("branch", out var br) ? br.GetString() : null,
            commit = runObj.TryGetProperty("commit", out var cm) ? cm.GetString() : null,
            startedUTC = startedUtc,
            endedUTC = endedUtc,
            totals = runObj.GetProperty("totals").Deserialize<object>()
        };

        await WriteJsonAsync(runFile, updated);

        var indexFile = Path.Combine(ReportRoot, "index.json");
        if (File.Exists(indexFile))
        {
            using var idxDoc = JsonDocument.Parse(await File.ReadAllTextAsync(indexFile));
            var idx = idxDoc.RootElement;
            var totals = idx.GetProperty("totals").Deserialize<object>();
            var updatedIdx = new
            {
                runID = idx.GetProperty("runID").GetString(),
                totals,
                durationMs = (long)(endedUtc - startedUtc).TotalMilliseconds,
                cases = idx.GetProperty("cases").Deserialize<object>()
            };
            await WriteJsonAsync(indexFile, updatedIdx);
        }
    }

    private static async Task HandleCaseStartedAsync(JsonElement root)
    {
        var caseId = root.GetProperty("caseID").GetGuid();
        var caseObj = new
        {
            runID = root.GetProperty("runID").GetString(),
            caseId,
            name = root.GetProperty("name").GetString(),
            className = root.GetProperty("className").GetString(),
            categories = root.GetProperty("categories").Deserialize<string[]>() ?? Array.Empty<string>(),
            correlationID = root.GetProperty("correlationID").GetString(),
            startedUTC = root.GetProperty("startedUTC").GetDateTime(),
            endedUTC = (DateTime?)null,
            status = (string?)null,
            durationMs = (long?)null
        };

        var caseDir = Path.Combine(ReportRoot, "cases");
        Directory.CreateDirectory(caseDir);
        await WriteJsonAsync(Path.Combine(caseDir, $"{caseId}.json"), caseObj);
    }

    private static async Task HandleCaseCompletedAsync(JsonElement root)
    {
        var caseId = root.GetProperty("caseID").GetGuid();
        var caseFile = Path.Combine(ReportRoot, "cases", $"{caseId}.json");
        if (!File.Exists(caseFile)) return;

        using var doc = JsonDocument.Parse(await File.ReadAllTextAsync(caseFile));
        var startedUtc = doc.RootElement.GetProperty("startedUTC").GetDateTime();
        var endedUtc = root.GetProperty("endedUTC").GetDateTime();
        var status = root.GetProperty("status").GetString();
        var durationMs = root.GetProperty("durationMs").GetInt64();

        var updated = new
        {
            runID = doc.RootElement.GetProperty("runID").GetString(),
            caseId,
            name = doc.RootElement.GetProperty("name").GetString(),
            className = doc.RootElement.GetProperty("className").GetString(),
            categories = doc.RootElement.GetProperty("categories").Deserialize<string[]>() ?? Array.Empty<string>(),
            correlationID = doc.RootElement.GetProperty("correlationID").GetString(),
            startedUTC = startedUtc,
            endedUTC = endedUtc,
            status,
            durationMs
        };

        await WriteJsonAsync(caseFile, updated);
        await UpdateTotalsAsync(
            updated.runID!, caseId.ToString(), updated.name!, updated.className!, updated.categories!, status!, durationMs);
    }

    private static async Task UpdateTotalsAsync(string runId, string caseId, string name, string className, string[] categories, string status, long durationMs)
    {
        var runFile = Path.Combine(ReportRoot, "run.json");
        if (File.Exists(runFile))
        {
            using var doc = JsonDocument.Parse(await File.ReadAllTextAsync(runFile));
            var run = doc.RootElement;
            var totals = run.GetProperty("totals");
            int passed = totals.GetProperty("passed").GetInt32();
            int failed = totals.GetProperty("failed").GetInt32();
            int skipped = totals.GetProperty("skipped").GetInt32();
            switch (status)
            {
                case "Passed": passed++; break;
                case "Failed": failed++; break;
                default: skipped++; break;
            }
            var updatedRun = new
            {
                runID = run.GetProperty("runID").GetString(),
                runName = run.TryGetProperty("runName", out var rn) ? rn.GetString() : null,
                machine = run.TryGetProperty("machine", out var mc) ? mc.GetString() : Environment.MachineName,
                branch = run.TryGetProperty("branch", out var br) ? br.GetString() : null,
                commit = run.TryGetProperty("commit", out var cm) ? cm.GetString() : null,
                startedUTC = run.GetProperty("startedUTC").GetDateTime(),
                endedUTC = run.TryGetProperty("endedUTC", out var eu) && eu.ValueKind != JsonValueKind.Null ? eu.GetDateTime() : (DateTime?)null,
                totals = new { passed, failed, skipped }
            };
            await WriteJsonAsync(runFile, updatedRun);
        }

        var indexFile = Path.Combine(ReportRoot, "index.json");
        if (File.Exists(indexFile))
        {
            using var doc = JsonDocument.Parse(await File.ReadAllTextAsync(indexFile));
            var idx = doc.RootElement;
            var totals = idx.GetProperty("totals");
            int passed = totals.GetProperty("passed").GetInt32();
            int failed = totals.GetProperty("failed").GetInt32();
            int skipped = totals.GetProperty("skipped").GetInt32();
            switch (status)
            {
                case "Passed": passed++; break;
                case "Failed": failed++; break;
                default: skipped++; break;
            }
            var casesArray = idx.GetProperty("cases").Deserialize<List<object>>() ?? new List<object>();
            casesArray.Add(new { caseId, name, status, durationMs, className, categories });
            var updatedIdx = new
            {
                runID = idx.GetProperty("runID").GetString(),
                totals = new { passed, failed, skipped },
                durationMs = idx.TryGetProperty("durationMs", out var dm) ? dm.GetInt64() : 0,
                cases = casesArray
            };
            await WriteJsonAsync(indexFile, updatedIdx);
        }
    }

    private static async Task HandleStepLoggedAsync(JsonElement root)
    {
        var caseId = root.GetProperty("caseID").GetGuid();
        var stepsDir = Path.Combine(ReportRoot, "steps");
        Directory.CreateDirectory(stepsDir);
        var file = Path.Combine(stepsDir, $"{caseId}.jsonl");
        await AppendJsonLineAsync(file, root);
    }

    private static async Task HandleLogWrittenAsync(JsonElement root)
    {
        var caseId = root.GetProperty("caseID").GetGuid();
        var logsDir = Path.Combine(ReportRoot, "logs");
        Directory.CreateDirectory(logsDir);
        var file = Path.Combine(logsDir, $"{caseId}.jsonl");
        await AppendJsonLineAsync(file, root);
    }

    private static Task HandleAttachmentAddedAsync(JsonElement root)
    {
        // Attachments are written by producers; engine keeps references only.
        return Task.CompletedTask;
    }

    private static async Task WriteJsonAsync(string path, object obj)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(path)!);
        var temp = path + ".tmp";
        await File.WriteAllTextAsync(temp, JsonSerializer.Serialize(obj, s_jsonOpts));
        if (File.Exists(path)) File.Delete(path);
        File.Move(temp, path);
    }

    private static async Task AppendJsonLineAsync(string path, JsonElement obj)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(path)!);
        await using var stream = new FileStream(path, FileMode.Append, FileAccess.Write, FileShare.Read);
        await using var writer = new StreamWriter(stream, new UTF8Encoding(false));
        await writer.WriteLineAsync(obj.GetRawText());
        await writer.FlushAsync();
    }
}


