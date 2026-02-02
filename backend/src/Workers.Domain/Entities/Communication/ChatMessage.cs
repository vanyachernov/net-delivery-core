using Workers.Domain.Common;
using Workers.Domain.Entities.Users;

namespace Workers.Domain.Entities.Communication;

/// <summary>
/// Represents a single message within a chat room.
/// </summary>
public class ChatMessage : BaseEntity
{
    /// <summary>
    /// ID of the room where this message was sent.
    /// </summary>
    public Guid ChatRoomId { get; set; }
    public ChatRoom ChatRoom { get; set; } = null!;
    
    /// <summary>
    /// ID of the user who sent the message.
    /// </summary>
    public Guid SenderId { get; set; }
    public User Sender { get; set; } = null!;
    
    /// <summary>
    /// Text content of the message.
    /// </summary>
    public string Content { get; set; } = string.Empty;

    /// <summary>
    /// Indicates if the recipient has seen the message.
    /// </summary>
    public bool IsRead { get; set; }
}
