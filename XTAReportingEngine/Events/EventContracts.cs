using System.Text.Json.Serialization;

namespace XTAReportingEngine.Events;

public abstract record XTAEvent
{
    [JsonPropertyName("eventType")] public string EventType { get; init; } = default!;
    [JsonPropertyName("runId")] public string RunId { get; init; } = default!;
    [JsonPropertyName("timestampUtc")] public DateTime TimestampUtc { get; init; }
}

public sealed record RunStartedEvent : XTAEvent
{
    [JsonPropertyName("runName")] public string? RunName { get; init; }
    [JsonPropertyName("machine")] public string Machine { get; init; } = Environment.MachineName;
    [JsonPropertyName("branch")] public string? Branch { get; init; }
    [JsonPropertyName("commit")] public string? Commit { get; init; }
}

public sealed record RunCompletedEvent : XTAEvent;

public sealed record CaseStartedEvent : XTAEvent
{
    [JsonPropertyName("caseId")] public Guid CaseId { get; init; }
    [JsonPropertyName("name")] public string Name { get; init; } = default!;
    [JsonPropertyName("className")] public string ClassName { get; init; } = default!;
    [JsonPropertyName("categories")] public string[] Categories { get; init; } = Array.Empty<string>();
    [JsonPropertyName("correlationId")] public string CorrelationId { get; init; } = default!;
    [JsonPropertyName("startedUtc")] public DateTime StartedUtc { get; init; }
}

public sealed record CaseCompletedEvent : XTAEvent
{
    [JsonPropertyName("caseId")] public Guid CaseId { get; init; }
    [JsonPropertyName("correlationId")] public string CorrelationId { get; init; } = default!;
    [JsonPropertyName("status")] public string Status { get; init; } = default!; // Passed|Failed|Skipped
    [JsonPropertyName("endedUtc")] public DateTime EndedUtc { get; init; }
    [JsonPropertyName("durationMs")] public long DurationMs { get; init; }
}

public sealed record StepLoggedEvent : XTAEvent
{
    [JsonPropertyName("caseId")] public Guid CaseId { get; init; }
    [JsonPropertyName("correlationId")] public string CorrelationId { get; init; } = default!;
    [JsonPropertyName("order")] public int Order { get; init; }
    [JsonPropertyName("name")] public string Name { get; init; } = default!;
    [JsonPropertyName("message")] public string? Message { get; init; }
    [JsonPropertyName("status")] public string Status { get; init; } = "Info"; // Info|Passed|Failed
    [JsonPropertyName("attachment")] public StepAttachment? Attachment { get; init; }
}

public sealed record LogWrittenEvent : XTAEvent
{
    [JsonPropertyName("caseId")] public Guid CaseId { get; init; }
    [JsonPropertyName("correlationId")] public string CorrelationId { get; init; } = default!;
    [JsonPropertyName("level")] public string Level { get; init; } = "Info"; // Info|Warn|Error
    [JsonPropertyName("message")] public string Message { get; init; } = default!;
    [JsonPropertyName("exception")] public string? Exception { get; init; }
}

public sealed record AttachmentAddedEvent : XTAEvent
{
    [JsonPropertyName("caseId")] public Guid CaseId { get; init; }
    [JsonPropertyName("correlationId")] public string CorrelationId { get; init; } = default!;
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
    public const string RunStarted = nameof(RunStarted);
    public const string RunCompleted = nameof(RunCompleted);
    public const string CaseStarted = nameof(CaseStarted);
    public const string CaseCompleted = nameof(CaseCompleted);
    public const string StepLogged = nameof(StepLogged);
    public const string LogWritten = nameof(LogWritten);
    public const string AttachmentAdded = nameof(AttachmentAdded);
}


