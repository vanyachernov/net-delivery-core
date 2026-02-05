using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Workers.Application.Common.Interfaces;
using Workers.Infrastructure.Persistence;
using Workers.Infrastructure.Persistence.Repositories;
using Workers.Infrastructure.Messaging;

namespace Workers.Infrastructure;

public static class DependencyInjection
{
    public static void AddInfrastructure(this IHostApplicationBuilder builder)
    {
        // Database
        builder.AddNpgsqlDbContext<ApplicationDbContext>("DefaultConnection",
            configureDbContextOptions: options =>
            {
                options.UseSnakeCaseNamingConvention();
            });
            
        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<ApplicationDbContext>());
        
        // Kafka - используем Aspire если доступен, иначе ручную конфигурацию
        AddKafkaInfrastructure(builder);
    }
    
    private static void AddKafkaInfrastructure(IHostApplicationBuilder builder)
    {
        // 1. Всегда загружаем базовые настройки из appsettings.json
        builder.Services.Configure<KafkaSettings>(
            builder.Configuration.GetSection(KafkaSettings.SectionName));

        // 2. Проверяем Aspire connection
        var connectionStrings = builder.Configuration.GetSection("ConnectionStrings");
        var aspireKafkaConnectionExists = connectionStrings.GetChildren()
            .Any(c => c.Key.Equals("kafka", StringComparison.OrdinalIgnoreCase));
        
        if (aspireKafkaConnectionExists)
        {
            // Получаем connection string от Aspire для обновления настроек Consumer
            var kafkaConnectionString = connectionStrings["kafka"];
            
            // Обновляем настройки KafkaSettings в рантайме, чтобы ConsumerService использовал правильный адрес
             builder.Services.PostConfigure<KafkaSettings>(settings =>
             {
                 settings.BootstrapServers = kafkaConnectionString!;
             });

            // Используем Aspire Kafka integration для Producer
            builder.AddKafkaProducer<string, string>("kafka", settings =>
            {
                settings.Config.EnableIdempotence = true;
                settings.Config.Acks = Confluent.Kafka.Acks.All;
                settings.Config.MessageSendMaxRetries = 3;
            });
            
            // Регистрируем wrapper через фабрику (используем Aspire Producer)
            builder.Services.AddSingleton<IKafkaProducer>(sp =>
            {
                var producer = sp.GetRequiredService<Confluent.Kafka.IProducer<string, string>>();
                var logger = sp.GetRequiredService<Microsoft.Extensions.Logging.ILogger<KafkaProducer>>();
                return new KafkaProducer(producer, logger);
            });
        }
        else
        {
            // Fallback на ручную конфигурацию (Producer создается нами вручную)
            builder.Services.AddSingleton<IKafkaProducer>(sp =>
            {
                var settings = sp.GetRequiredService<Microsoft.Extensions.Options.IOptions<KafkaSettings>>();
                var logger = sp.GetRequiredService<Microsoft.Extensions.Logging.ILogger<KafkaProducer>>();
                return new KafkaProducer(settings, logger);
            });
        }
    }
    
    /// <summary>
    /// Регистрация Kafka Consumer
    /// </summary>
    /// <summary>
    /// Регистрация Kafka Consumer с конкретной реализацией обработчика
    /// </summary>
    public static void AddKafkaConsumer<TConsumer, TMessage>(
        this IServiceCollection services,
        string groupId,
        params string[] topics) 
        where TConsumer : class, IKafkaConsumer<TMessage>
        where TMessage : class
    {
        // 1. Регистрируем сам обработчик (Consumer Logic) как Scoped
        services.AddScoped<IKafkaConsumer<TMessage>, TConsumer>();

        // 2. Регистрируем Background Service, который будет читать Kafka и вызывать обработчик
        services.AddHostedService(sp =>
        {
            var settings = sp.GetRequiredService<Microsoft.Extensions.Options.IOptions<KafkaSettings>>();
            var logger = sp.GetRequiredService<Microsoft.Extensions.Logging.ILogger<KafkaConsumerService<TMessage>>>();
            return new KafkaConsumerService<TMessage>(sp, settings, logger, groupId, topics);
        });
    }
}
