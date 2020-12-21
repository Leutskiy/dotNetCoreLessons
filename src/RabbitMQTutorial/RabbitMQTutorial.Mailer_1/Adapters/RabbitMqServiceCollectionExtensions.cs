using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.ObjectPool;
using RabbitMQ.Client;
using RabbitMQTutorial.Mailer_1.Infrastructure;

using SendSmsNotification = RabbitMQTutorial.Mailer_1.Features.SendSms.SendSms.Notification;

namespace RabbitMQTutorial.Mailer_1.Adapters
{
	public static class RabbitMqServiceCollectionExtensions
    {
        public static IServiceCollection AddRabbitMq(this IServiceCollection services, IConfiguration configuration)
        {
            var rabbitMqConnection = new RabbitMqConnectionSettings();
            configuration.GetSection("RabbitMq:Connection").Bind(rabbitMqConnection);
            services.AddSingleton(rabbitMqConnection);

            var rabbitMqRouting = new RabbitMqPubSubSettings();
            configuration.GetSection("RabbitMq:Routing").Bind(rabbitMqRouting);
            services.AddSingleton(rabbitMqRouting);

            services.AddSingleton<ObjectPoolProvider, DefaultObjectPoolProvider>();
            services.AddSingleton<IPooledObjectPolicy<IModel>, RabbitMqModelPooledObjectPolicyAsync>();

            return services;
        }

        public static IServiceCollection AddRabbitMqPriorityQueueForMessage(this IServiceCollection services)
        {
            services.AddSingleton<IProducer<PriorityIntegrationEvent<SendSmsNotification>>, SmsNotificationProducer>();

            services.AddHostedService<SmsNotificationConsumer>();

            return services;
        }
    }
}
