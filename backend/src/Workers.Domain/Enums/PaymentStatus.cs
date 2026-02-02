namespace Workers.Domain.Enums;

/// <summary>
/// Defines the various states of a payment transaction.
/// </summary>
public enum PaymentStatus
{
    Pending = 1,
    Success = 2,
    Failed = 3
}
