using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using XTAReportingEngine.Events;

namespace XTAInfras.XReporting;

public static class XTAReportTestEvents
{
    private const string m_EXCHANGE = "xta.test.events.topic";

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
        // await channel.ExchangeDeclareAsync(m_EXCHANGE, ExchangeType.Topic, durable: true, cancellationToken: ct);
        
        var evt = new TestStartedEvent
        {
            EventType = XTAEventTypes.TestStarted,
            RunSessionID = runSessionID,
            TimestampUTC = DateTime.UtcNow,
            TestMethodName = testMethodName,
            TestClassName = testClassName,
            TestCategories = testCategories,
            CorrelationID = correlationID,
            StartedUTC = startedUTC
        };

        await m_PublishAsync(
            channel, 
            evt, 
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
        var evtC = new TestCompletedEvent
        {
            EventType = XTAEventTypes.TestCompleted,
            RunSessionID = runSessionID,
            TimestampUTC = DateTime.UtcNow,
            CorrelationID = correlationID,
            Status = status,
            EndedUTC = endedUTC,
            DurationMs = durationMs
        };

        await m_PublishAsync(channel, evtC, $"run.{runSessionID}", ct);
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
        var evtS = new StepLoggedEvent
        {
            EventType = XTAEventTypes.StepLogged,
            RunSessionID = runID,
            TimestampUTC = DateTime.UtcNow,
            CorrelationID = correlationID,
            Order = order,
            Name = name,
            Message = message,
            Status = status,
            Attachment = attachmentRelativePath is null ? null : new StepAttachment { Kind = "Screenshot", RelativePath = attachmentRelativePath, Caption = attachmentCaption }
        };

        await m_PublishAsync(channel, evtS, $"run.{runID}", ct);
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
        var evtL = new LogWrittenEvent
        {
            EventType = XTAEventTypes.LogWritten,
            RunSessionID = runSessionID,
            TimestampUTC = DateTime.UtcNow,
            CorrelationID = correlationID,
            Level = level,
            Message = message,
            Exception = exception
        };
        
        await m_PublishAsync(channel, evtL, $"run.{runSessionID}", ct);
    }

    private static async Task m_PublishAsync(IChannel channel, object payload, string routingKey, CancellationToken ct)
    {
        var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(payload));
        var props = new BasicProperties { Persistent = true, ContentType = "application/json" };
        
        await channel.BasicPublishAsync(
            m_EXCHANGE, routingKey, mandatory: false, basicProperties: props, body: body.AsMemory(), ct);
    }
}


