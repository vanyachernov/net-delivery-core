using Microsoft.Extensions.Logging;
using Workers.Application.Common.Interfaces;
using Workers.Domain.Events;
using Workers.Infrastructure.Messaging;

namespace Workers.Infrastructure.Messaging.Consumers;

/// <summary>
/// Расширенный пример обработчика событий создания пользователя
/// Показывает реальную бизнес-логику
/// </summary>
public class UserCreatedEventConsumerExample : IKafkaConsumer<UserCreatedEvent>
{
    private readonly ILogger<UserCreatedEventConsumerExample> _logger;
    // В реальном проекте инжектите нужные сервисы:
    // private readonly IEmailService _emailService;
    // private readonly IUserProfileService _profileService;
    // private readonly IAnalyticsService _analyticsService;
    // private readonly INotificationService _notificationService;

    public UserCreatedEventConsumerExample(ILogger<UserCreatedEventConsumerExample> logger)
    {
        _logger = logger;
    }

    public IEnumerable<string> Topics => new[] { KafkaTopics.UserEvents };
    public string GroupId => "user-created-consumer-group";

    public async Task HandleAsync(UserCreatedEvent message, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "═══════════════════════════════════════════════════════════");
        _logger.LogInformation(
            "🎉 Получено событие UserCreatedEvent");
        _logger.LogInformation(
            "   EventId: {EventId}", message.EventId);
        _logger.LogInformation(
            "   UserId: {UserId}", message.UserId);
        _logger.LogInformation(
            "   Email: {Email}", message.Email);
        _logger.LogInformation(
            "   Name: {Name}", message.Name);
        _logger.LogInformation(
            "   CreatedAt: {CreatedAt}", message.CreatedAt);
        _logger.LogInformation(
            "═══════════════════════════════════════════════════════════");

        try
        {
            // ═══════════════════════════════════════════════════════════
            // 1️⃣ Отправить Welcome Email
            // ═══════════════════════════════════════════════════════════
            _logger.LogInformation("📧 Отправка welcome email на {Email}...", message.Email);
            
            // await _emailService.SendWelcomeEmailAsync(
            //     to: message.Email,
            //     userName: message.Name,
            //     cancellationToken: cancellationToken
            // );
            
            await Task.Delay(100, cancellationToken); // Имитация отправки
            _logger.LogInformation("✅ Welcome email отправлен");

            // ═══════════════════════════════════════════════════════════
            // 2️⃣ Создать профиль пользователя с дефолтными настройками
            // ═══════════════════════════════════════════════════════════
            _logger.LogInformation("👤 Создание профиля пользователя...");
            
            // await _profileService.CreateDefaultProfileAsync(
            //     userId: message.UserId,
            //     settings: new UserSettings
            //     {
            //         Language = "ru",
            //         Timezone = "Europe/Moscow",
            //         EmailNotifications = true,
            //         PushNotifications = true
            //     },
            //     cancellationToken: cancellationToken
            // );
            
            await Task.Delay(50, cancellationToken); // Имитация создания
            _logger.LogInformation("✅ Профиль создан");

            // ═══════════════════════════════════════════════════════════
            // 3️⃣ Отправить событие в аналитику
            // ═══════════════════════════════════════════════════════════
            _logger.LogInformation("📊 Отправка события в аналитику...");
            
            // await _analyticsService.TrackEventAsync(
            //     eventName: "user_registered",
            //     userId: message.UserId,
            //     properties: new Dictionary<string, object>
            //     {
            //         ["email"] = message.Email,
            //         ["registration_date"] = message.CreatedAt,
            //         ["source"] = "api"
            //     },
            //     cancellationToken: cancellationToken
            // );
            
            await Task.Delay(50, cancellationToken); // Имитация отправки
            _logger.LogInformation("✅ Событие отправлено в аналитику");

            // ═══════════════════════════════════════════════════════════
            // 4️⃣ Отправить push-уведомление (если есть токен)
            // ═══════════════════════════════════════════════════════════
            _logger.LogInformation("🔔 Создание приветственного уведомления...");
            
            // await _notificationService.CreateNotificationAsync(
            //     userId: message.UserId,
            //     title: "Добро пожаловать!",
            //     message: $"Привет, {message.Name}! Рады видеть тебя в нашем сервисе.",
            //     type: NotificationType.InApp,
            //     cancellationToken: cancellationToken
            // );
            
            await Task.Delay(50, cancellationToken); // Имитация создания
            _logger.LogInformation("✅ Уведомление создано");

            // ═══════════════════════════════════════════════════════════
            // 5️⃣ Добавить в CRM систему (опционально)
            // ═══════════════════════════════════════════════════════════
            _logger.LogInformation("💼 Синхронизация с CRM...");
            
            // await _crmService.CreateContactAsync(
            //     email: message.Email,
            //     name: message.Name,
            //     tags: new[] { "new_user", "registered_via_api" },
            //     cancellationToken: cancellationToken
            // );
            
            await Task.Delay(50, cancellationToken); // Имитация синхронизации
            _logger.LogInformation("✅ Контакт создан в CRM");

            // ═══════════════════════════════════════════════════════════
            // ✅ Успешная обработка
            // ═══════════════════════════════════════════════════════════
            _logger.LogInformation(
                "═══════════════════════════════════════════════════════════");
            _logger.LogInformation(
                "✅ Событие UserCreatedEvent успешно обработано для {UserId}",
                message.UserId);
            _logger.LogInformation(
                "═══════════════════════════════════════════════════════════");
        }
        catch (Exception ex)
        {
            // ═══════════════════════════════════════════════════════════
            // ❌ Ошибка при обработке
            // ═══════════════════════════════════════════════════════════
            _logger.LogError(ex,
                "❌ Ошибка при обработке UserCreatedEvent для {UserId}. " +
                "Событие будет повторно обработано.",
                message.UserId);

            // Опции обработки ошибок:
            
            // 1. Re-throw - Kafka повторит обработку
            throw;
            
            // 2. Отправить в Dead Letter Queue
            // await _kafkaProducer.ProduceAsync(
            //     KafkaTopics.DeadLetterQueue,
            //     message
            // );
            
            // 3. Логировать и пропустить (не рекомендуется)
            // return;
        }
    }
}

/// <summary>
/// Пример настроек пользователя
/// </summary>
public class UserSettings
{
    public string Language { get; set; } = "en";
    public string Timezone { get; set; } = "UTC";
    public bool EmailNotifications { get; set; } = true;
    public bool PushNotifications { get; set; } = false;
}
