namespace Workers.Domain.Events;

/// <summary>
/// Базовый класс для событий Kafka
/// </summary>
public abstract record KafkaEvent
{
    /// <summary>
    /// Уникальный идентификатор события
    /// </summary>
    public Guid EventId { get; init; } = Guid.NewGuid();
    
    /// <summary>
    /// Время создания события
    /// </summary>
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    
    /// <summary>
    /// Тип события
    /// </summary>
    public string EventType { get; init; } = string.Empty;
}
