using MediatR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.ObjectPool;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQTutorial.Mailer_1.Adapters;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using static System.Text.Encoding;
using SendSmsCommand = RabbitMQTutorial.Mailer_1.Features.SendSms.SendSms.Command;
using SendSmsNotification = RabbitMQTutorial.Mailer_1.Features.SendSms.SendSms.Notification;

namespace RabbitMQTutorial.Mailer_1.Infrastructure
{
	/// <summary>
	/// Потребитель сообщений из очереди с приоритетом RabbitMQ
	/// </summary>
	public sealed class SmsNotificationConsumer : RabbitMqConsumerBase<SendSmsCommand>, IHostedService
	{
		private readonly static string XMaxPriorityKey = "x-max-priority";
		private readonly static object XMaxPriorityVal = 3;

		private readonly static List<SendSmsNotification> _commands = new List<SendSmsNotification>();

		private readonly ILogger<SmsNotificationConsumer> _logger;

		public SmsNotificationConsumer(
			ILogger<SmsNotificationConsumer> logger,
			IMediator mediator,
			IPooledObjectPolicy<IModel> pooledObjectPolicy,
			RabbitMqPubSubSettings rabbitMqPubSubSettings)
			: base(
				  mediator,
				  pooledObjectPolicy,
				  rabbitMqPubSubSettings,
				  logger,
				  new[] { (key: XMaxPriorityKey, val: XMaxPriorityVal) } )
		{
			_logger = logger;

			Listen();
		}

		public Task StartAsync(CancellationToken cancellationToken) => Task.CompletedTask;

		public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

		/// <summary>
		/// Асинхронный обработчик получения сообщения из очереди RabbitMQ
		/// </summary>
		/// <param name="sender">Инициатор вызова асинхронного обработчика</param>
		/// <param name="eventArgs">Событие доаставки сообщения</param>
		/// <returns>Awaitable-объект</returns>
		protected override async Task OnEventReceivedAsync(object sender, BasicDeliverEventArgs eventArgs)
		{
			var channel = ChannelPool.Get();

			try
			{
				var messageByteArray = eventArgs.Body.ToArray();
				var body = UTF8.GetString(messageByteArray);
				var message = JsonConvert.DeserializeObject<PriorityIntegrationEvent<SendSmsNotification>>(body);


				if (_commands.Count >= 9)
				{
					var result = await Mediator.Send(new SendSmsCommand
					{
						Notifications = _commands.ToArray()
					});

					_logger.LogInformation($"The command has returned the following ids: {string.Join(", ", result.NotificationIds)}");
					channel.BasicAck(eventArgs.DeliveryTag, true);
					_logger.LogInformation("The message is deleted from the messaging queue");

					_commands.Clear();
				}

				_logger.LogInformation($"Priority: {message.Priority}");
				_commands.Add(message.Data);
			}
			catch (Exception ex)
			{
				_logger.LogCritical(ex, "Error while retrieving message from queue.");
			}
			finally
			{
				ChannelPool.Return(channel);
			}

			await Task.Yield();
		}

		private void Listen()
		{
			var channel = ChannelPool.Get();

			try
			{
				channel.BasicQos(0, 10, false);
				var consumer = new AsyncEventingBasicConsumer(channel);
				consumer.Received += OnEventReceivedAsync;
				channel.BasicConsume(queue: QueueName, autoAck: false, consumer: consumer);
			}
			catch (Exception ex)
			{
				_logger.LogCritical(ex, "Error while consuming message");
			}
			finally
			{
				ChannelPool.Return(channel);
			}
		}
	}
}
