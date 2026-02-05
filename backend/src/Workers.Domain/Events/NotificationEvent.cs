namespace Workers.Domain.Events;

/// <summary>
/// Событие для отправки уведомлений
/// </summary>
public record NotificationEvent : KafkaEvent
{
    public NotificationEvent()
    {
        EventType = nameof(NotificationEvent);
    }
    
    /// <summary>
    /// ID получателя
    /// </summary>
    public Guid RecipientId { get; init; }
    
    /// <summary>
    /// Заголовок уведомления
    /// </summary>
    public string Title { get; init; } = string.Empty;
    
    /// <summary>
    /// Текст уведомления
    /// </summary>
    public string Message { get; init; } = string.Empty;
    
    /// <summary>
    /// Тип уведомления (email, push, sms)
    /// </summary>
    public NotificationType Type { get; init; }
    
    /// <summary>
    /// Дополнительные данные
    /// </summary>
    public Dictionary<string, string>? Metadata { get; init; }
}

public enum NotificationType
{
    Email,
    Push,
    Sms,
    InApp
}
