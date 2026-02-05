using Microsoft.AspNetCore.Mvc;
using Workers.Application.Common.Interfaces;
using Workers.Domain.Events;
using Workers.Infrastructure.Messaging;

namespace Workers.Api.Controllers;

/// <summary>
/// Пример контроллера для демонстрации работы с Kafka
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class KafkaExampleController : ControllerBase
{
    private readonly IKafkaProducer _kafkaProducer;
    private readonly ILogger<KafkaExampleController> _logger;

    public KafkaExampleController(
        IKafkaProducer kafkaProducer,
        ILogger<KafkaExampleController> logger)
    {
        _kafkaProducer = kafkaProducer;
        _logger = logger;
    }

    /// <summary>
    /// Отправить тестовое событие создания пользователя
    /// </summary>
    [HttpPost("user-created")]
    public async Task<IActionResult> SendUserCreatedEvent(
        [FromBody] SendUserCreatedRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            var userCreatedEvent = new UserCreatedEvent
            {
                UserId = Guid.NewGuid(),
                Email = request.Email,
                Name = request.Name
            };

            await _kafkaProducer.ProduceAsync(
                KafkaTopics.UserEvents,
                userCreatedEvent.UserId.ToString(),
                userCreatedEvent,
                cancellationToken);

            _logger.LogInformation(
                "UserCreatedEvent sent for email {Email}",
                request.Email);

            return Ok(new
            {
                Message = "Event sent successfully",
                EventId = userCreatedEvent.EventId,
                UserId = userCreatedEvent.UserId
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending UserCreatedEvent");
            return StatusCode(500, "Failed to send event");
        }
    }

    /// <summary>
    /// Отправить тестовое уведомление
    /// </summary>
    [HttpPost("notification")]
    public async Task<IActionResult> SendNotification(
        [FromBody] SendNotificationRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            var notificationEvent = new NotificationEvent
            {
                RecipientId = request.RecipientId,
                Title = request.Title,
                Message = request.Message,
                Type = request.Type,
                Metadata = request.Metadata
            };

            await _kafkaProducer.ProduceAsync(
                KafkaTopics.Notifications,
                notificationEvent.RecipientId.ToString(),
                notificationEvent,
                cancellationToken);

            _logger.LogInformation(
                "NotificationEvent sent to recipient {RecipientId}",
                request.RecipientId);

            return Ok(new
            {
                Message = "Notification event sent successfully",
                EventId = notificationEvent.EventId
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending NotificationEvent");
            return StatusCode(500, "Failed to send notification");
        }
    }
}

public record SendUserCreatedRequest(string Email, string Name);

public record SendNotificationRequest(
    Guid RecipientId,
    string Title,
    string Message,
    NotificationType Type,
    Dictionary<string, string>? Metadata = null);
