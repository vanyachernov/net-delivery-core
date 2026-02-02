namespace Workers.Domain.Enums;

/// <summary>
/// Defines the worker's current availability status for taking on tasks.
/// </summary>
public enum AvailabilityStatus
{
    Available = 1,
    Working = 2,
    PartlyAvailable = 3,
    Unavailable = 4
}
