using Workers.Domain.Common;
using Workers.Domain.Enums;
using Workers.Domain.Entities.Users;
using Workers.Domain.Entities.Categories;
using Workers.Domain.Entities.Locations;
using Workers.Domain.Entities.Workers;

namespace Workers.Domain.Entities.Communication;

/// <summary>
/// Represents a job request created by a client looking for a specialist.
/// </summary>
public class WorkRequest : BaseEntity
{
    /// <summary>
    /// Short title description of the job.
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Detailed description of the work needed.
    /// </summary>
    public string Description { get; set; } = string.Empty;
    
    /// <summary>
    /// User who created the request.
    /// </summary>
    public Guid ClientId { get; set; }
    public User Client { get; set; } = null!;
    
    /// <summary>
    /// Targeted service category.
    /// </summary>
    public Guid CategoryId { get; set; }
    public Category Category { get; set; } = null!;
    
    /// <summary>
    /// Where the work should be performed.
    /// </summary>
    public Guid LocationId { get; set; }
    public Location Location { get; set; } = null!;
    
    /// <summary>
    /// Current status (Open, In Progress, etc.).
    /// </summary>
    public WorkRequestStatus Status { get; set; } = WorkRequestStatus.Open;

    /// <summary>
    /// Estimated budget for the project.
    /// </summary>
    public decimal? Budget { get; set; }
    
    /// <summary>
    /// Desired date for service execution.
    /// </summary>
    public DateTime? PreferredDate { get; set; }

    /// <summary>
    /// Specific tags for the job to help AI matching (e.g., "urgent", "apartment", "renovation").
    /// </summary>
    public string[] Tags { get; set; } = [];
    
    public ICollection<Offer> Offers { get; set; } = [];
}