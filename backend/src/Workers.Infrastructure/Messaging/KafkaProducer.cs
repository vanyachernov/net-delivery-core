using System.Text.Json;
using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Workers.Application.Common.Interfaces;

namespace Workers.Infrastructure.Messaging;

/// <summary>
/// Реализация Kafka Producer
/// </summary>
public class KafkaProducer : IKafkaProducer, IDisposable
{
    private readonly IProducer<string, string> _producer;
    private readonly ILogger<KafkaProducer> _logger;
    private readonly bool _isEnabled;
    private readonly bool _ownsProducer;
    private bool _disposed;

    // Конструктор для Aspire (DI инжектит готовый Producer)
    public KafkaProducer(
        IProducer<string, string> producer,
        ILogger<KafkaProducer> logger)
    {
        _producer = producer;
        _logger = logger;
        _isEnabled = true;
        _ownsProducer = false; // Aspire управляет lifecycle
    }

    // Конструктор для ручной конфигурации (фоллбэк)
    public KafkaProducer(
        IOptions<KafkaSettings> settings,
        ILogger<KafkaProducer> logger)
    {
        var kafkaSettings = settings.Value;
        _logger = logger;
        _isEnabled = kafkaSettings.Enabled;
        _ownsProducer = true;

        if (!_isEnabled)
        {
            // Создаём dummy producer для избежания null reference
            _producer = new ProducerBuilder<string, string>(new ProducerConfig
            {
                BootstrapServers = "localhost:9092"
            }).Build();
            return;
        }

        var config = new ProducerConfig
        {
            BootstrapServers = kafkaSettings.BootstrapServers,
            Acks = ParseAcks(kafkaSettings.Producer.Acks),
            EnableIdempotence = kafkaSettings.Producer.EnableIdempotence,
            MessageSendMaxRetries = kafkaSettings.Producer.MessageSendMaxRetries,
            MessageTimeoutMs = kafkaSettings.Producer.MessageTimeoutMs,
            // Дополнительные настройки для надежности
            RequestTimeoutMs = 30000,
            RetryBackoffMs = 100,
            CompressionType = CompressionType.Snappy,
        };

        _producer = new ProducerBuilder<string, string>(config)
            .SetErrorHandler((_, error) =>
            {
                _logger.LogError("Kafka producer error: {ErrorReason}", error.Reason);
            })
            .SetLogHandler((_, logMessage) =>
            {
                var logLevel = logMessage.Level switch
                {
                    SyslogLevel.Emergency or SyslogLevel.Alert or SyslogLevel.Critical or SyslogLevel.Error => LogLevel.Error,
                    SyslogLevel.Warning => LogLevel.Warning,
                    SyslogLevel.Notice or SyslogLevel.Info => LogLevel.Information,
                    _ => LogLevel.Debug
                };
                _logger.Log(logLevel, "Kafka: {Message}", logMessage.Message);
            })
            .Build();
    }

    public async Task ProduceAsync<T>(string topic, string key, T message, CancellationToken cancellationToken = default)
        where T : class
    {
        if (!_isEnabled)
        {
            _logger.LogWarning("Kafka is disabled. Message to topic {Topic} was not sent", topic);
            return;
        }

        try
        {
            var serializedMessage = JsonSerializer.Serialize(message, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            var kafkaMessage = new Message<string, string>
            {
                Key = key,
                Value = serializedMessage,
                Timestamp = Timestamp.Default
            };

            var deliveryResult = await _producer.ProduceAsync(topic, kafkaMessage, cancellationToken);

            _logger.LogInformation(
                "Message delivered to topic {Topic}, partition {Partition}, offset {Offset}",
                deliveryResult.Topic,
                deliveryResult.Partition.Value,
                deliveryResult.Offset.Value);
        }
        catch (ProduceException<string, string> ex)
        {
            _logger.LogError(ex, "Error producing message to topic {Topic}: {ErrorReason}", topic, ex.Error.Reason);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error producing message to topic {Topic}", topic);
            throw;
        }
    }

    public Task ProduceAsync<T>(string topic, T message, CancellationToken cancellationToken = default)
        where T : class
    {
        // Используем Guid как ключ по умолчанию
        return ProduceAsync(topic, Guid.NewGuid().ToString(), message, cancellationToken);
    }

    private static Acks ParseAcks(string acks) => acks.ToLowerInvariant() switch
    {
        "0" or "none" => Acks.None,
        "1" or "leader" => Acks.Leader,
        "all" or "-1" => Acks.All,
        _ => Acks.All
    };

    public void Dispose()
    {
        if (_disposed)
            return;

        // Dispose только если producer создан нами, а не Aspire
        if (_ownsProducer)
        {
            _producer?.Flush(TimeSpan.FromSeconds(10));
            _producer?.Dispose();
        }
        
        _disposed = true;
        GC.SuppressFinalize(this);
    }
}
