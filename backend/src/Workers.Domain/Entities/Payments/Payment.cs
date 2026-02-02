using Workers.Domain.Common;
using Workers.Domain.Entities.Users;
using Workers.Domain.Enums;

namespace Workers.Domain.Entities.Payments;

/// <summary>
/// Represents a financial transaction for services or platform features.
/// </summary>
public class Payment : BaseEntity
{
    /// <summary>
    /// Link to the user who made the payment.
    /// </summary>
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
    
    /// <summary>
    /// Monetary amount of the transaction.
    /// </summary>
    public decimal Amount { get; set; }

    /// <summary>
    /// Currency code (default: EUR).
    /// </summary>
    public string Currency { get; set; } = "EUR";
    
    /// <summary>
    /// Transaction ID from the external gateway (Stripe/PayPal).
    /// </summary>
    public string ExternalTransactionId { get; set; } = string.Empty;

    /// <summary>
    /// Payment state (Pending, Success, Failed).
    /// </summary>
    public PaymentStatus Status { get; set; } = PaymentStatus.Pending;

    /// <summary>
    /// Purpose of the payment (Boost, Subscription, etc.).
    /// </summary>
    public PaymentType Type { get; set; }
}
