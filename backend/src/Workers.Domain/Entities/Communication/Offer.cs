using Workers.Domain.Common;
using Workers.Domain.Entities.Workers;

namespace Workers.Domain.Entities.Communication;

/// <summary>
/// Represents a worker's response or bid for a specific work request.
/// </summary>
public class Offer : BaseEntity
{
    /// <summary>
    /// ID of the targeted work request.
    /// </summary>
    public Guid WorkRequestId { get; set; }
    public WorkRequest WorkRequest { get; set; } = null!;
    
    /// <summary>
    /// ID of the worker profile making the offer.
    /// </summary>
    public Guid WorkerProfileId { get; set; }
    public WorkerProfile WorkerProfile { get; set; } = null!;
    
    /// <summary>
    /// Message explaining the offer or expressing interest.
    /// </summary>
    public string? Message { get; set; }

    /// <summary>
    /// Price proposed by the worker for the job.
    /// </summary>
    public decimal ProposedPrice { get; set; }
    
    /// <summary>
    /// Indicates if the client has accepted this specific offer.
    /// </summary>
    public bool IsAccepted { get; set; }
}
