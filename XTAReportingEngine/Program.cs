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

    private static string ReportRoot
        => Environment.GetEnvironmentVariable("XTA_REPORT_ROOT") is { Length: > 0 } custom
            ? custom
            : Path.Combine(AppContext.BaseDirectory, "report");

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
        consumer.ReceivedAsync += async (object _, BasicDeliverEventArgs ea) =>
        {
            try
            {
                var json = Encoding.UTF8.GetString(ea.Body.ToArray());
                var root = JsonDocument.Parse(json).RootElement;
                var eventType = root.GetProperty("eventType").GetString();

                switch (eventType)
                {
                    case nameof(XTAEventTypes.RunSessionStarted):
                        await HandleRunStartedAsync(root);
                        break;
                    case nameof(XTAEventTypes.RunSessionCompleted):
                        await HandleRunCompletedAsync(root);
                        break;
                    case nameof(XTAEventTypes.TestStarted):
                        await HandleTestStartedAsync(root);
                        break;
                    case nameof(XTAEventTypes.TestCompleted):
                        await HandleTestCompletedAsync(root);
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
            runSessionID = root.GetProperty("runSessionID").GetString(),
            runMode = root.TryGetProperty("runMode", out var rm) ? rm.GetString() : null,
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
            runSessionID = run.runSessionID,
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
            runSessionID = runObj.GetProperty("runSessionID").GetString(),
            runMode = runObj.TryGetProperty("runMode", out var rm) ? rm.GetString() : null,
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
                runSessionID = idx.GetProperty("runSessionID").GetString(),
                totals,
                durationMs = (long)(endedUtc - startedUtc).TotalMilliseconds,
                cases = idx.GetProperty("cases").Deserialize<object>()
            };
            await WriteJsonAsync(indexFile, updatedIdx);
        }
    }

    private static async Task HandleTestStartedAsync(JsonElement root)
    {
        var correlationId = root.GetProperty("correlationID").GetString()!;
        var caseObj = new
        {
            runSessionID = root.GetProperty("runSessionID").GetString(),
            correlationID = correlationId,
            testMethodName = root.GetProperty("testMethodName").GetString(),
            testClassName = root.GetProperty("testClassName").GetString(),
            testCategories = root.GetProperty("testCategories").Deserialize<string[]>() ?? Array.Empty<string>(),
            startedUTC = root.GetProperty("startedUTC").GetDateTime(),
            endedUTC = (DateTime?)null,
            status = (string?)null,
            durationMs = (long?)null
        };

        var caseDir = Path.Combine(ReportRoot, "cases");
        Directory.CreateDirectory(caseDir);
        await WriteJsonAsync(Path.Combine(caseDir, $"{correlationId}.json"), caseObj);
    }

    private static async Task HandleTestCompletedAsync(JsonElement root)
    {
        var correlationId = root.GetProperty("correlationID").GetString()!;
        var caseFile = Path.Combine(ReportRoot, "cases", $"{correlationId}.json");
        if (!File.Exists(caseFile)) return;

        using var doc = JsonDocument.Parse(await File.ReadAllTextAsync(caseFile));
        var startedUtc = doc.RootElement.GetProperty("startedUTC").GetDateTime();
        var endedUtc = root.GetProperty("endedUTC").GetDateTime();
        var status = root.GetProperty("status").GetString();
        var durationMs = root.GetProperty("durationMs").GetInt64();

        var updated = new
        {
            runSessionID = doc.RootElement.GetProperty("runSessionID").GetString(),
            correlationID = correlationId,
            testMethodName = doc.RootElement.GetProperty("testMethodName").GetString(),
            testClassName = doc.RootElement.GetProperty("testClassName").GetString(),
            testCategories = doc.RootElement.GetProperty("testCategories").Deserialize<string[]>() ?? Array.Empty<string>(),
            startedUTC = startedUtc,
            endedUTC = endedUtc,
            status,
            durationMs
        };

        await WriteJsonAsync(caseFile, updated);
        
        await UpdateTotalsAsync(
            updated.runSessionID!, correlationId, updated.testMethodName!, updated.testClassName!, updated.testCategories!, status!, durationMs);
    }

    private static async Task UpdateTotalsAsync(string runSessionID, string caseKey, string testMethodName, string testClassName, string[] testCategories, string status, long durationMs)
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
                runSessionID = run.GetProperty("runSessionID").GetString(),
                runMode = run.TryGetProperty("runMode", out var rm) ? rm.GetString() : null,
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
            casesArray.Add(new { caseKey, testMethodName, status, durationMs, testClassName, testCategories });
            var updatedIdx = new
            {
                runSessionID = idx.GetProperty("runSessionID").GetString(),
                totals = new { passed, failed, skipped },
                durationMs = idx.TryGetProperty("durationMs", out var dm) ? dm.GetInt64() : 0,
                cases = casesArray
            };
            await WriteJsonAsync(indexFile, updatedIdx);
        }
    }

    private static async Task HandleStepLoggedAsync(JsonElement root)
    {
        var correlationId = root.GetProperty("correlationID").GetString()!;
        var stepsDir = Path.Combine(ReportRoot, "steps");
        Directory.CreateDirectory(stepsDir);
        var file = Path.Combine(stepsDir, $"{correlationId}.jsonl");
        await AppendJsonLineAsync(file, root);
    }

    private static async Task HandleLogWrittenAsync(JsonElement root)
    {
        var correlationId = root.GetProperty("correlationID").GetString()!;
        var logsDir = Path.Combine(ReportRoot, "logs");
        Directory.CreateDirectory(logsDir);
        var file = Path.Combine(logsDir, $"{correlationId}.jsonl");
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


