using System;
using System.Text;
using System.Threading.Tasks;
using RabbitMQTutorial.Mailer_1.Infrastructure;
using MediatR;
using Microsoft.Extensions.ObjectPool;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace RabbitMQTutorial.Mailer_1.Adapters
{
    /// <summary>
    /// Базовый класс потребителя сообщений RabbitMQ
    /// </summary>
    /// <typeparam name="TMessageBody">Cообщение из RabbitMQ</typeparam>
    public abstract class RabbitMqConsumerBase<TMessageBody> : RabbitMqClientBase
    {
		private readonly IMediator _mediator;
        private readonly ILogger _logger;

		/// <summary>
		/// Конструктор с параметрами
		/// </summary>
		/// <param name="mediator">Посредник Джимми Б.</param>
		/// <param name="pooledChannelPolicy">Политика управления каналами в пуле</param>
		/// <param name="options">Общии настройки подключения для publisher/consumer</param>
		/// <param name="args"></param>
		/// <param name="logger"></param>
		public RabbitMqConsumerBase(
            IMediator mediator,
            IPooledObjectPolicy<IModel> pooledChannelPolicy,
            RabbitMqPubSubSettings options,
            ILogger logger,
            (string key, object val)[]? args = default)
            : base(pooledChannelPolicy, options, args)
        {
            _mediator = mediator;
			_logger = logger;
		}

		protected IMediator Mediator => _mediator;

		/// <summary>
		/// Асинхронный обработчик получения сообщения из очереди RabbitMQ
		/// </summary>
		/// <param name="sender">Инициатор вызова асинхронного обработчика</param>
		/// <param name="eventArgs">Событие доаставки сообщения</param>
		/// <returns>Awaitable-объект</returns>
		protected virtual async Task OnEventReceivedAsync(object sender, BasicDeliverEventArgs eventArgs)
        {
            var channel = ChannelPool.Get();

            try
            {
                var body = Encoding.UTF8.GetString(eventArgs.Body.ToArray());
                var message = JsonConvert.DeserializeObject<TMessageBody>(body);

                await Mediator.Send(message);

                channel.BasicAck(eventArgs.DeliveryTag, false);
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Error while retrieving message from queue.");
            }
            finally
            {
                ChannelPool.Return(channel);
                await Task.Yield();
            }
        }
    }
}
