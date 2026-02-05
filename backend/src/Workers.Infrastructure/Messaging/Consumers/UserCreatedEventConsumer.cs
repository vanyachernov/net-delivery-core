using Microsoft.Extensions.Logging;
using Workers.Application.Common.Interfaces;
using Workers.Domain.Events;
using Workers.Infrastructure.Messaging;

namespace Workers.Infrastructure.Messaging.Consumers;

/// <summary>
/// Обработчик событий создания пользователя
/// Пример: можно отправить welcome email, создать профиль и т.д.
/// </summary>
public class UserCreatedEventConsumer : IKafkaConsumer<UserCreatedEvent>
{
    private readonly ILogger<UserCreatedEventConsumer> _logger;
    // Здесь можно инжектить другие сервисы (например, IEmailService, IUserRepository и т.д.)

    public UserCreatedEventConsumer(ILogger<UserCreatedEventConsumer> logger)
    {
        _logger = logger;
    }

    public IEnumerable<string> Topics => new[] { KafkaTopics.UserEvents };
    public string GroupId => "user-created-consumer-group";

    public async Task HandleAsync(UserCreatedEvent message, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation(
            "Processing UserCreatedEvent for user {UserId} ({Email})",
            message.UserId,
            message.Email);

        try
        {
            // TODO: Здесь добавить бизнес-логику:
            // - Отправить welcome email
            // - Создать запись в аналитике
            // - Инициализировать профиль пользователя
            // - Отправить событие в систему аудита
            
            await Task.CompletedTask; // Заглушка для async

            _logger.LogInformation(
                "Successfully processed UserCreatedEvent for user {UserId}",
                message.UserId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, 
                "Failed to process UserCreatedEvent for user {UserId}",
                message.UserId);
            throw; // Re-throw для retry логики
        }
    }
}
