namespace Workers.Domain.Enums;

/// <summary>
/// Defines the current stage of a work request (job).
/// </summary>
public enum WorkRequestStatus
{
    Open = 1,
    InProgress = 2,
    Completed = 3,
    Cancelled = 4
}
