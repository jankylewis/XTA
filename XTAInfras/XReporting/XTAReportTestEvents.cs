using System.Text;
using System.Text.Json;
using RabbitMQ.Client;

namespace XTAInfras.XReporting;

public static class XTAReportTestEvents
{
    private const string m_EXCHANGE = "xta.test.events";

    public static async Task PublishTestStartedAsync(
        IChannel channel,
        string runSessionID,
        string testMethodName,
        string testClassName,
        string[] testCategories,
        string correlationID,
        DateTime startedUTC,
        CancellationToken ct = default
        )
    {
        await channel.ExchangeDeclareAsync(m_EXCHANGE, ExchangeType.Direct, durable: true, cancellationToken: ct);
        
        var payload = new
        {
            eventType = "TestStarted",
            runSessionID,
            timestampUTC = DateTime.UtcNow,
            testMethodName,
            testClassName,
            testCategories,
            correlationID,
            startedUTC
        };

        await m_PublishAsync(
            channel, 
            payload, 
            routingKey: $"run.{runSessionID}", 
            ct
            );
    }

    public static async Task PublishTestCompletedAsync(
        IChannel channel, 
        string runSessionID, 
        string correlationID, 
        string status, 
        DateTime endedUTC, 
        long durationMs, 
        CancellationToken ct = default
        )
    {
        var payload = new
        {
            eventType = "TestCompleted",
            runSessionID,
            timestampUTC = DateTime.UtcNow,
            correlationID,
            status,
            endedUTC,
            durationMs
        };

        await m_PublishAsync(channel, payload, $"run.{runSessionID}", ct);
    }

    public static async Task PublishStepLoggedAsync(
        IChannel channel, 
        string runID, 
        string correlationID, 
        int order, 
        string name, 
        string? message, 
        string status, 
        string? attachmentRelativePath = null, 
        string? attachmentCaption = null, 
        CancellationToken ct = default)
    {
        var payload = new
        {
            eventType = "StepLogged",
            runID,
            timestampUTC = DateTime.UtcNow,
            correlationID,
            order,
            name,
            message,
            status,
            attachment = attachmentRelativePath is null ? null : new { kind = "Screenshot", relativePath = attachmentRelativePath, caption = attachmentCaption }
        };

        await m_PublishAsync(channel, payload, $"run.{runID}", ct);
    }

    public static async Task PublishLogWrittenAsync(
        IChannel channel, 
        string runSessionID, 
        string correlationID, 
        string level, 
        string message, 
        string? exception = null, 
        CancellationToken ct = default)
    {
        var payload = new
        {
            eventType = "LogWritten",
            runSessionID,
            timestampUTC = DateTime.UtcNow,
            correlationID,
            level,
            message,
            exception
        };
        
        await m_PublishAsync(channel, payload, $"run.{runSessionID}", ct);
    }

    private static async Task m_PublishAsync(IChannel channel, object payload, string routingKey, CancellationToken ct)
    {
        var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(payload));
        var props = new BasicProperties { Persistent = true, ContentType = "application/json" };
        
        await channel.BasicPublishAsync(
            m_EXCHANGE, routingKey, mandatory: false, basicProperties: props, body: body.AsMemory(), ct);
    }
}


