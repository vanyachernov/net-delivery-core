using Workers.Domain.Common;
using Workers.Domain.Enums;

namespace Workers.Domain.Entities.Workers;

/// <summary>
/// Represents media files (images, videos, certificates) attached to a worker's profile.
/// </summary>
public class WorkerMedia : BaseEntity
{
    /// <summary>
    /// ID of the associated worker profile.
    /// </summary>
    public Guid WorkerProfileId { get; set; }
    public WorkerProfile WorkerProfile { get; set; } = null!;
    
    /// <summary>
    /// Direct URL to the media file in storage.
    /// </summary>
    public string Url { get; set; } = string.Empty;

    /// <summary>
    /// URL to a smaller preview image (for videos or large images).
    /// </summary>
    public string? ThumbnailUrl { get; set; }

    /// <summary>
    /// Type of the media (Image, Video, Certificate).
    /// </summary>
    public MediaType Type { get; set; }

    /// <summary>
    /// Indicates if the media is visible to the public (Portfolio) or private for verification.
    /// </summary>
    public bool IsPublic { get; set; } = true;
}
