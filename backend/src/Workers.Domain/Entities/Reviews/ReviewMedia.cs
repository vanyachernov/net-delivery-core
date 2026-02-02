using Workers.Domain.Common;

namespace Workers.Domain.Entities.Reviews;

/// <summary>
/// Represents media (photos) attached to a review.
/// </summary>
public class ReviewMedia : BaseEntity
{
    /// <summary>
    /// ID of the review this media belongs to.
    /// </summary>
    public Guid ReviewId { get; set; }
    public Review Review { get; set; } = null!;

    /// <summary>
    /// URL to the photo or media file.
    /// </summary>
    public string Url { get; set; } = string.Empty;
}
