using System;
using Microsoft.Extensions.ObjectPool;
using Newtonsoft.Json;
using RabbitMQ.Client;

using static System.Text.Encoding;
using static System.Net.Mime.MediaTypeNames.Application;
using Microsoft.Extensions.Logging;

namespace RabbitMQTutorial.Mailer_1.Adapters
{
	/// <summary>
	/// Базовый класс производителя сообщений RabbitMq
	/// </summary>
	/// <typeparam name="TEvent">Интеграционное событие</typeparam>
	public abstract class RabbitMqProducerBase<TEvent> : RabbitMqClientBase, IProducer<TEvent>
        where TEvent : IntegrationEventBase
    {
		private readonly ILogger _logger;

		/// <summary>
		/// Констурктор
		/// </summary>
		/// <param name="pooledChannelPolicy">Политика управления каналами в пуле</param>
		/// <param name="options">Общии настройки подключения для publisher/consumer</param>
		/// <param name="logger"></param>
		/// <param name="args"></param>
		protected RabbitMqProducerBase(
            IPooledObjectPolicy<IModel> pooledChannelPolicy,
            RabbitMqPubSubSettings options,
            ILogger logger,
            (string key, object val)[]? args = default)
            : base(pooledChannelPolicy, options, args) 
        {
			_logger = logger;
		}

        /// <summary>
        /// Идентификатор приложения-производителя
        /// </summary>
        protected abstract string AppId { get; }

        /// <summary>
        /// Опубликовать интеграционное событие
        /// </summary>
        /// <param name="event">Интеграционное событие</param>
        public virtual void Publish(TEvent @event)
        {
            var channel = ChannelPool.Get();

            try
            {
                var body = UTF8.GetBytes(JsonConvert.SerializeObject(@event));

                var basicProperties = channel.CreateBasicProperties();
                SetBasicProperties(basicProperties, @event);

                channel.BasicPublish(exchange: ExchangeName, routingKey: RouteKeyName, body: body, basicProperties: basicProperties);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while publishing");
            }
            finally
            {
                ChannelPool.Return(channel);
            }
        }

        /// <summary>
        /// Установить базовые свойства
        /// </summary>
        /// <param name="basicProperties">Базовые свойства</param>
        /// <param name="event">Интеграционное событие</param>
        protected virtual void SetBasicProperties(IBasicProperties basicProperties, TEvent @event)
        {
            basicProperties.AppId = AppId;
            basicProperties.ContentType = Json;

            basicProperties.MessageId = $"{@event.Id}";
            basicProperties.Timestamp = new AmqpTimestamp(@event.Timestamp.ToUnixTimeSeconds());
        }
    }
}
