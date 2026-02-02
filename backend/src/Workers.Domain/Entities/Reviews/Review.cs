using Workers.Domain.Common;
using Workers.Domain.Entities.Users;
using Workers.Domain.Entities.Communication;

namespace Workers.Domain.Entities.Reviews;

/// <summary>
/// Represents a review left by a user for another user after a work request is completed.
/// </summary>
public class Review : BaseEntity
{
    /// <summary>
    /// ID of the related work request.
    /// </summary>
    public Guid WorkRequestId { get; set; }
    public WorkRequest WorkRequest { get; set; } = null!;
    
    /// <summary>
    /// ID of the user who left the review.
    /// </summary>
    public Guid ReviewerId { get; set; }
    public User Reviewer { get; set; } = null!;
    
    /// <summary>
    /// ID of the user who received the review.
    /// </summary>
    public Guid RevieweeId { get; set; }
    public User Reviewee { get; set; } = null!;
    
    /// <summary>
    /// Aggregate rating (1-5), often calculated from the sub-criteria.
    /// </summary>
    public int AverageRating { get; set; }

    /// <summary>
    /// Quality of the work performed (1-5).
    /// </summary>
    public int QualityRating { get; set; }

    /// <summary>
    /// Punctuality and communication speed (1-5).
    /// </summary>
    public int PunctualityRating { get; set; }

    /// <summary>
    /// Fairness of the price for the value provided (1-5).
    /// </summary>
    public int PriceValueRating { get; set; }

    /// <summary>
    /// Text comment of the review.
    /// </summary>
    public string? Comment { get; set; }
    
    public ICollection<ReviewMedia> Media { get; set; } = [];
}
