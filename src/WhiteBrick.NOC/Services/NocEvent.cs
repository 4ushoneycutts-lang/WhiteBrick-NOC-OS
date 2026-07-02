namespace WhiteBrick.NOC.Services;

public enum NocEventSeverity
{
    Info,
    Success,
    Warning,
    Error
}

public sealed record NocEvent(
    DateTime Timestamp,
    string Source,
    string Message,
    NocEventSeverity Severity);