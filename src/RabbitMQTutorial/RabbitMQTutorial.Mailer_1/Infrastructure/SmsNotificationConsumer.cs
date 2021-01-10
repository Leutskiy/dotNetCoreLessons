using MediatR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.ObjectPool;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQTutorial.Mailer_1.Adapters;
using System;
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
		private static int _counter = 1;


		private readonly static string XMaxPriorityKey = "x-max-priority";
		private readonly static object XMaxPriorityVal = 3;

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

				_logger.LogInformation($"Priority: {message.Priority}");

				var result = await Mediator.Send(new SendSmsCommand
				{
					Notification = message.Data
				});

				_logger.LogInformation($"The command has returned the following id: {result.NotificationId}");


				if (eventArgs.Redelivered)
				{
					_logger.LogInformation($"         ONLY REDELIVERED!!!            ");
				}
				


				if ((_counter++) % 5 == 0)
				{
					throw new Exception("This is thrown the test exception");
				}

				channel.BasicAck(eventArgs.DeliveryTag, false);

				_logger.LogInformation("The message is deleted from the messaging queue");


			}
			catch (Exception ex)
			{
				_logger.LogCritical(ex, "Error while retrieving message from queue.");
				var requeue = false;
				channel.BasicNack(eventArgs.DeliveryTag, false, requeue);
				_logger.LogInformation("The message goes back in queue");
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
				channel.BasicQos(0, 1, false);
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
