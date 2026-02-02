using Workers.Domain.Common;
using Workers.Domain.Enums;
using Workers.Domain.Entities.Users;
using Workers.Domain.Entities.Categories;
using Workers.Domain.Entities.Locations;

namespace Workers.Domain.Entities.Workers;

/// <summary>
/// Detailed profile for a worker (master), containing bio, portfolio, and availability status.
/// </summary>
public class WorkerProfile : BaseEntity
{
    /// <summary>
    /// Link to the user who owns this profile.
    /// </summary>
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
    
    /// <summary>
    /// Professional biography or description.
    /// </summary>
    public string? Bio { get; set; }

    /// <summary>
    /// External website or social media link.
    /// </summary>
    public string? Website { get; set; }

    /// <summary>
    /// Current availability for work orders.
    /// </summary>
    public AvailabilityStatus AvailabilityStatus { get; set; } = AvailabilityStatus.Available;
    
    /// <summary>
    /// Aggregate rating based on user reviews.
    /// </summary>
    public decimal Rating { get; set; }

    /// <summary>
    /// Total count of reviews received.
    /// </summary>
    public int ReviewsCount { get; set; }

    /// <summary>
    /// Total number of profile views.
    /// </summary>
    public long TotalViews { get; set; }
    
    /// <summary>
    /// Indicates if the profile has passed identity verification.
    /// </summary>
    public bool IsVerified { get; set; }

    /// <summary>
    /// Internal status of the verification process.
    /// </summary>
    public VerificationStatus VerificationStatus { get; set; } = VerificationStatus.NotVerified;
    
    /// <summary>
    /// Maximum distance (in km) the worker is willing to travel for a job.
    /// </summary>
    public int ServingRadiusKm { get; set; }

    /// <summary>
    /// Specialized skills/tags used for AI matching and search (e.g., "installation", "repair").
    /// </summary>
    public string[] Tags { get; set; } = [];

    /// <summary>
    /// Automated status: indicates when the worker will be available if currently busy.
    /// </summary>
    public DateTime? AvailableUntil { get; set; }

    /// <summary>
    /// Indicates if the user paid for profile boosting.
    /// </summary>
    public bool IsBoosted { get; set; }

    /// <summary>
    /// Expiration date of the manual/paid boost.
    /// </summary>
    public DateTime? BoostUntil { get; set; }
    
    /// <summary>
    /// Main location of the worker.
    /// </summary>
    public Guid? LocationId { get; set; }
    public Location? Location { get; set; }

    /// <summary>
    /// Collection of portfolio items showcasing past work.
    /// </summary>
    public ICollection<WorkerPortfolioItem> PortfolioItems { get; set; } = [];

    /// <summary>
    /// Collection of photos, videos, and certificates.
    /// </summary>
    public ICollection<WorkerMedia> Media { get; set; } = [];

    /// <summary>
    /// Collection of categories the worker specializes in.
    /// </summary>
    public ICollection<WorkerCategory> Categories { get; set; } = [];
}
}
