namespace Workers.Application.Common.Interfaces;

/// <summary>
/// Базовый интерфейс для обработчиков Kafka сообщений
/// </summary>
public interface IKafkaConsumer
{
    /// <summary>
    /// Топики для подписки
    /// </summary>
    IEnumerable<string> Topics { get; }
    
    /// <summary>
    /// Название consumer group
    /// </summary>
    string GroupId { get; }
}

/// <summary>
/// Типизированный обработчик Kafka сообщений
/// </summary>
public interface IKafkaConsumer<T> : IKafkaConsumer where T : class
{
    /// <summary>
    /// Обработать сообщение
    /// </summary>
    Task HandleAsync(T message, CancellationToken cancellationToken = default);
}
