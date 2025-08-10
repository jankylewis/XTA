using System.Text.Json.Serialization;

namespace XTAReportingEngine.Events;

public abstract record XTAEvent
{
    [JsonPropertyName("eventType")] public string EventType { get; init; } = default!;
    [JsonPropertyName("runID")] public string RunID { get; init; } = default!;
    [JsonPropertyName("timestampUTC")] public DateTime TimestampUTC { get; init; }
}

public sealed record RunStartedEvent : XTAEvent
{
    [JsonPropertyName("runName")] public string? RunName { get; init; }
    [JsonPropertyName("machine")] public string Machine { get; init; } = Environment.MachineName;
    [JsonPropertyName("branch")] public string? Branch { get; init; }
    [JsonPropertyName("commit")] public string? Commit { get; init; }
}

public sealed record RunCompletedEvent : XTAEvent;

public sealed record TestStartedEvent : XTAEvent
{
    [JsonPropertyName("testMethodName")] public string TestMethodName { get; init; } = default!;
    [JsonPropertyName("testClassName")] public string TestClassName { get; init; } = default!;
    [JsonPropertyName("testCategories")] public string[] TestCategories { get; init; } = Array.Empty<string>();
    [JsonPropertyName("correlationID")] public string CorrelationID { get; init; } = default!;
    [JsonPropertyName("startedUTC")] public DateTime StartedUTC { get; init; }
}

public sealed record TestCompletedEvent : XTAEvent
{
    [JsonPropertyName("correlationID")] public string CorrelationID { get; init; } = default!;
    [JsonPropertyName("status")] public string Status { get; init; } = default!; // Passed|Failed|Skipped
    [JsonPropertyName("endedUTC")] public DateTime EndedUTC { get; init; }
    [JsonPropertyName("durationMs")] public long DurationMs { get; init; }
}

public sealed record StepLoggedEvent : XTAEvent
{
    [JsonPropertyName("correlationID")] public string CorrelationID { get; init; } = default!;
    [JsonPropertyName("order")] public int Order { get; init; }
    [JsonPropertyName("name")] public string Name { get; init; } = default!;
    [JsonPropertyName("message")] public string? Message { get; init; }
    [JsonPropertyName("status")] public string Status { get; init; } = "Info"; // Info|Passed|Failed
    [JsonPropertyName("attachment")] public StepAttachment? Attachment { get; init; }
}

public sealed record LogWrittenEvent : XTAEvent
{
    [JsonPropertyName("correlationID")] public string CorrelationID { get; init; } = default!;
    [JsonPropertyName("level")] public string Level { get; init; } = "Info"; // Info|Warn|Error
    [JsonPropertyName("message")] public string Message { get; init; } = default!;
    [JsonPropertyName("exception")] public string? Exception { get; init; }
}

public sealed record AttachmentAddedEvent : XTAEvent
{
    [JsonPropertyName("correlationID")] public string CorrelationID { get; init; } = default!;
    [JsonPropertyName("kind")] public string Kind { get; init; } = default!; // Screenshot|Log|Other
    [JsonPropertyName("relativePath")] public string RelativePath { get; init; } = default!;
    [JsonPropertyName("caption")] public string? Caption { get; init; }
}

public sealed record StepAttachment
{
    [JsonPropertyName("kind")] public string Kind { get; init; } = "Screenshot";
    [JsonPropertyName("relativePath")] public string RelativePath { get; init; } = default!;
    [JsonPropertyName("caption")] public string? Caption { get; init; }
}

public static class XTAEventTypes
{
    public const string RunSessionStarted = nameof(RunSessionStarted);
    public const string RunSessionCompleted = nameof(RunSessionCompleted);
    public const string TestStarted = nameof(TestStarted);
    public const string TestCompleted = nameof(TestCompleted);
    public const string StepLogged = nameof(StepLogged);
    public const string LogWritten = nameof(LogWritten);
    public const string AttachmentAdded = nameof(AttachmentAdded);
}


