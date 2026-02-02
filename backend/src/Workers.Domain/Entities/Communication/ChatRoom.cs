using Workers.Domain.Common;

namespace Workers.Domain.Entities.Communication;

/// <summary>
/// Represents a conversation space between a client and a worker.
/// </summary>
public class ChatRoom : BaseEntity
{
    /// <summary>
    /// Optional ID of the work request associated with this chat.
    /// </summary>
    public Guid? WorkRequestId { get; set; }
    public WorkRequest? WorkRequest { get; set; }
    
    /// <summary>
    /// All messages sent within this chat room.
    /// </summary>
    public ICollection<ChatMessage> Messages { get; set; } = [];
}
