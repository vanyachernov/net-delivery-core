using Workers.Domain.Common;

namespace Workers.Domain.Entities.Workers;

/// <summary>
/// Represents a work sample or project in a worker's portfolio.
/// </summary>
public class WorkerPortfolioItem : BaseEntity
{
    /// <summary>
    /// ID of the worker profile this portfolio item belongs to.
    /// </summary>
    public Guid WorkerProfileId { get; set; }
    public WorkerProfile WorkerProfile { get; set; } = null!;
    
    /// <summary>
    /// Title of the project or work.
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Detailed description of the work performed.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Image URL showing the state before work started.
    /// </summary>
    public string? BeforeImageUrl { get; set; }

    /// <summary>
    /// Image URL showing the finished result.
    /// </summary>
    public string? AfterImageUrl { get; set; }

    /// <summary>
    /// Date when the project was completed.
    /// </summary>
    public DateTime? CompletionDate { get; set; }
}
