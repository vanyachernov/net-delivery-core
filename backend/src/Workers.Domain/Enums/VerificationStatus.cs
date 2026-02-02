namespace Workers.Domain.Enums;

/// <summary>
/// Status of the worker's account or document verification process.
/// </summary>
public enum VerificationStatus
{
    NotVerified = 0,
    Pending = 1,
    Verified = 2,
    Rejected = 3
}
