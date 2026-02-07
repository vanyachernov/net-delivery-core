using Workers.Domain.Common;
using Workers.Domain.Enums;
using Workers.Domain.Entities.Workers;
using Workers.Domain.Entities.Companies;
using Microsoft.AspNetCore.Identity;

namespace Workers.Domain.Entities.Users;

/// <summary>
/// Represents a user within the platform, including authentication flags and role information.
/// </summary>
public class User : IdentityUser<Guid>, IBaseEntity
{
    public UserRole Role { get; set; }

    /// <summary>
    /// User's first name.
    /// </summary>
    public string? FirstName { get; set; }

    /// <summary>
    /// User's last name.
    /// </summary>
    public string? LastName { get; set; }
    
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

    /// <summary>
    /// Refresh Token for generating new access tokens without login.
    /// </summary>
    public string? RefreshToken { get; set; }

    /// <summary>
    /// Expiry time for the Refresh Token.
    /// </summary>
    public DateTime? RefreshTokenExpiryTime { get; set; }

    #region IBaseEntity implementation

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
    public bool IsDeleted { get; set; } = false;

    #endregion
}
