namespace Workers.Infrastructure.Messaging;

/// <summary>
/// Настройки для подключения к Kafka
/// </summary>
public class KafkaSettings
{
    public const string SectionName = "Kafka";
    
    /// <summary>
    /// Bootstrap серверы (например: localhost:9092)
    /// </summary>
    public string BootstrapServers { get; set; } = "localhost:9092";
    
    /// <summary>
    /// Включена ли Kafka
    /// </summary>
    public bool Enabled { get; set; } = true;
    
    /// <summary>
    /// Настройки Producer
    /// </summary>
    public ProducerSettings Producer { get; set; } = new();
    
    /// <summary>
    /// Настройки Consumer
    /// </summary>
    public ConsumerSettings Consumer { get; set; } = new();
}

public class ProducerSettings
{
    /// <summary>
    /// Тип подтверждения записи (0, 1, all)
    /// </summary>
    public string Acks { get; set; } = "all";
    
    /// <summary>
    /// Включить идемпотентность
    /// </summary>
    public bool EnableIdempotence { get; set; } = true;
    
    /// <summary>
    /// Максимальное количество повторных попыток
    /// </summary>
    public int MessageSendMaxRetries { get; set; } = 3;
    
    /// <summary>
    /// Таймаут отправки сообщения в мс
    /// </summary>
    public int MessageTimeoutMs { get; set; } = 30000;
}

public class ConsumerSettings
{
    /// <summary>
    /// Auto offset reset (earliest, latest, none)
    /// </summary>
    public string AutoOffsetReset { get; set; } = "earliest";
    
    /// <summary>
    /// Включить автоматический коммит offset
    /// </summary>
    public bool EnableAutoCommit { get; set; } = false;
    
    /// <summary>
    /// Интервал автокоммита в мс
    /// </summary>
    public int AutoCommitIntervalMs { get; set; } = 5000;
    
    /// <summary>
    /// Максимальное время ожидания poll в мс
    /// </summary>
    public int MaxPollIntervalMs { get; set; } = 300000;
    
    /// <summary>
    /// Интервал heartbeat в мс
    /// </summary>
    public int HeartbeatIntervalMs { get; set; } = 3000;
    
    /// <summary>
    /// Таймаут сессии в мс
    /// </summary>
    public int SessionTimeoutMs { get; set; } = 45000;
}
