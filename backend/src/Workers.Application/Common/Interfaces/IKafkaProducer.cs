namespace Workers.Application.Common.Interfaces;

/// <summary>
/// Интерфейс для отправки сообщений в Kafka
/// </summary>
public interface IKafkaProducer
{
    /// <summary>
    /// Отправить сообщение в топик
    /// </summary>
    Task ProduceAsync<T>(string topic, string key, T message, CancellationToken cancellationToken = default) where T : class;
    
    /// <summary>
    /// Отправить сообщение в топик без ключа
    /// </summary>
    Task ProduceAsync<T>(string topic, T message, CancellationToken cancellationToken = default) where T : class;
}
