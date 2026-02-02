using Workers.Domain.Common;
using Workers.Domain.Enums;
using Workers.Domain.Entities.Workers;
using Workers.Domain.Entities.Companies;

namespace Workers.Domain.Entities.Users;

/// <summary>
/// Represents a user within the platform, including authentication flags and role information.
/// </summary>
public class User : BaseEntity
{
    /// <summary>
    /// Primary email address used for login and notifications.
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Phone number for SMS notifications and verification.
    /// </summary>
    public string PhoneNumber { get; set; } = string.Empty;

    /// <summary>
    /// User's first name.
    /// </summary>
    public string? FirstName { get; set; }

    /// <summary>
    /// User's last name.
    /// </summary>
    public string? LastName { get; set; }

    /// <summary>
    /// Access role of the user (Client, Worker, or Firma).
    /// </summary>
    public UserRole Role { get; set; }
    
    /// <summary>
    /// Whether the email address has been verified via link.
    /// </summary>
    public bool IsEmailVerified { get; set; }

    /// <summary>
    /// Whether the phone number has been verified via OTP.
    /// </summary>
    public bool IsPhoneVerified { get; set; }
    
    /// <summary>
    /// URL to the user's avatar image.
    /// </summary>
    public string? AvatarUrl { get; set; }
    
    /// <summary>
    /// Associated worker profile if the user is a specialist.
    /// </summary>
    public WorkerProfile? WorkerProfile { get; set; }

    /// <summary>
    /// Associated company profile if the user owns or manages a Company.
    /// </summary>
    public Company? Company { get; set; }
}
