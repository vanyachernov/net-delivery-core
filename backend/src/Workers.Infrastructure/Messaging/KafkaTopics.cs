namespace Workers.Infrastructure.Messaging;

/// <summary>
/// Константы для Kafka топиков
/// </summary>
public static class KafkaTopics
{
    /// <summary>
    /// Топик для событий пользователей
    /// </summary>
    public const string UserEvents = "user-events";
    
    /// <summary>
    /// Топик для уведомлений
    /// </summary>
    public const string Notifications = "notifications";
    
    /// <summary>
    /// Топик для email сообщений
    /// </summary>
    public const string EmailEvents = "email-events";
    
    /// <summary>
    /// Топик для audit событий
    /// </summary>
    public const string AuditEvents = "audit-events";
    
    /// <summary>
    /// Dead Letter Queue для необработанных сообщений
    /// </summary>
    public const string DeadLetterQueue = "dlq";
}
