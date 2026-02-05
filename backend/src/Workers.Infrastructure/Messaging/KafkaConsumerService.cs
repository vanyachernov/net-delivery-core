using System.Text.Json;
using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Workers.Application.Common.Interfaces;

namespace Workers.Infrastructure.Messaging;

/// <summary>
/// Базовый класс для Kafka Consumer с автоматической обработкой сообщений
/// </summary>
public class KafkaConsumerService<TMessage> : BackgroundService where TMessage : class
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<KafkaConsumerService<TMessage>> _logger;
    private readonly KafkaSettings _settings;
    private readonly string _groupId;
    private readonly IEnumerable<string> _topics;
    private IConsumer<string, string>? _consumer;

    public KafkaConsumerService(
        IServiceProvider serviceProvider,
        IOptions<KafkaSettings> settings,
        ILogger<KafkaConsumerService<TMessage>> logger,
        string groupId,
        IEnumerable<string> topics)
    {
        _serviceProvider = serviceProvider;
        _settings = settings.Value;
        _logger = logger;
        _groupId = groupId;
        _topics = topics;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (!_settings.Enabled)
        {
            _logger.LogWarning("Kafka is disabled. Consumer {GroupId} will not start", _groupId);
            return;
        }

        var config = new ConsumerConfig
        {
            BootstrapServers = _settings.BootstrapServers,
            GroupId = _groupId,
            AutoOffsetReset = ParseAutoOffsetReset(_settings.Consumer.AutoOffsetReset),
            EnableAutoCommit = _settings.Consumer.EnableAutoCommit,
            AutoCommitIntervalMs = _settings.Consumer.AutoCommitIntervalMs,
            MaxPollIntervalMs = _settings.Consumer.MaxPollIntervalMs,
            HeartbeatIntervalMs = _settings.Consumer.HeartbeatIntervalMs,
            SessionTimeoutMs = _settings.Consumer.SessionTimeoutMs,
            // Дополнительные настройки
            EnableAutoOffsetStore = false,
            // Разрешаем автосоздание топиков для упрощения разработки
            AllowAutoCreateTopics = true,
        };

        _logger.LogInformation("Starting Kafka consumer for group {GroupId} with servers: {Servers}", _groupId, _settings.BootstrapServers);

        _consumer = new ConsumerBuilder<string, string>(config)
            .SetErrorHandler((_, error) =>
            {
                _logger.LogError("Kafka consumer error in group {GroupId}: {ErrorReason}", _groupId, error.Reason);
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
                _logger.Log(logLevel, "Kafka consumer {GroupId}: {Message}", _groupId, logMessage.Message);
            })
            .Build();

        try
        {
            _consumer.Subscribe(_topics);
            _logger.LogInformation("Kafka consumer {GroupId} subscribed to topics: {Topics}", _groupId, string.Join(", ", _topics));

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var consumeResult = _consumer.Consume(stoppingToken);

                    if (consumeResult?.Message == null)
                        continue;

                    _logger.LogDebug(
                        "Received message from topic {Topic}, partition {Partition}, offset {Offset}",
                        consumeResult.Topic,
                        consumeResult.Partition.Value,
                        consumeResult.Offset.Value);

                    await ProcessMessageAsync(consumeResult.Message.Value, stoppingToken);

                    // Коммитим offset только после успешной обработки
                    _consumer.StoreOffset(consumeResult);
                    if (!_settings.Consumer.EnableAutoCommit)
                    {
                        _consumer.Commit(consumeResult);
                    }

                    _logger.LogDebug("Message from offset {Offset} processed successfully", consumeResult.Offset.Value);
                }
                catch (ConsumeException ex)
                {
                    _logger.LogError(ex, "Error consuming message in group {GroupId}: {ErrorReason}", _groupId, ex.Error.Reason);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Unexpected error processing message in group {GroupId}", _groupId);
                    // Здесь можно добавить логику повторной обработки или отправки в dead letter queue
                }
            }
        }
        catch (OperationCanceledException)
        {
            _logger.LogInformation("Kafka consumer {GroupId} is stopping", _groupId);
        }
        finally
        {
            _consumer.Close();
            _consumer.Dispose();
        }
    }

    private async Task ProcessMessageAsync(string messageValue, CancellationToken cancellationToken)
    {
        TMessage? message;
        try
        {
            message = JsonSerializer.Deserialize<TMessage>(messageValue, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            if (message == null)
            {
                _logger.LogWarning("Failed to deserialize message: null result");
                return;
            }
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, "Failed to deserialize message: {Message}", messageValue);
            return;
        }

        // Создаем новый scope для обработки сообщения
        using var scope = _serviceProvider.CreateScope();
        var handler = scope.ServiceProvider.GetService<IKafkaConsumer<TMessage>>();

        if (handler == null)
        {
            _logger.LogWarning("No handler registered for message type {MessageType}", typeof(TMessage).Name);
            return;
        }

        await handler.HandleAsync(message, cancellationToken);
    }

    private static AutoOffsetReset ParseAutoOffsetReset(string value) => value.ToLowerInvariant() switch
    {
        "earliest" => AutoOffsetReset.Earliest,
        "latest" => AutoOffsetReset.Latest,
        "none" => AutoOffsetReset.Error,
        _ => AutoOffsetReset.Earliest
    };

    public override void Dispose()
    {
        _consumer?.Dispose();
        base.Dispose();
        GC.SuppressFinalize(this);
    }
}
