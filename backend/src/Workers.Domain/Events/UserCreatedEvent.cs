namespace Workers.Domain.Events;

/// <summary>
/// Событие создания пользователя
/// </summary>
public record UserCreatedEvent : KafkaEvent
{
    public UserCreatedEvent()
    {
        EventType = nameof(UserCreatedEvent);
    }
    
    /// <summary>
    /// ID пользователя
    /// </summary>
    public Guid UserId { get; init; }
    
    /// <summary>
    /// Email пользователя
    /// </summary>
    public string Email { get; init; } = string.Empty;
    
    /// <summary>
    /// Имя пользователя
    /// </summary>
    public string Name { get; init; } = string.Empty;
}
