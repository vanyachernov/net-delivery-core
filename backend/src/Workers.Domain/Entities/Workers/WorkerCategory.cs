using Workers.Domain.Common;
using Workers.Domain.Entities.Categories;

namespace Workers.Domain.Entities.Workers;

/// <summary>
/// Link between a worker and a service category they specialize in.
/// </summary>
public class WorkerCategory : BaseEntity
{
    /// <summary>
    /// ID of the associated worker profile.
    /// </summary>
    public Guid WorkerProfileId { get; set; }
    public WorkerProfile WorkerProfile { get; set; } = null!;
    
    /// <summary>
    /// ID of the category.
    /// </summary>
    public Guid CategoryId { get; set; }
    public Category Category { get; set; } = null!;
    
    /// <summary>
    /// Number of years of experience the worker has in this specific category.
    /// </summary>
    public int ExperienceYears { get; set; }
}
