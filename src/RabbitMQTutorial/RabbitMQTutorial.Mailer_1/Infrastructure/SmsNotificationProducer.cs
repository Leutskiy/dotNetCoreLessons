using Microsoft.Extensions.Logging;
using Microsoft.Extensions.ObjectPool;
using RabbitMQ.Client;
using RabbitMQTutorial.Mailer_1.Adapters;

using SendSmsNotification = RabbitMQTutorial.Mailer_1.Features.SendSms.SendSms.Notification;

namespace RabbitMQTutorial.Mailer_1.Infrastructure
{
	/// <summary>
	/// Производитель сообщений для очереди с приоритетом RabbitMq
	/// </summary>
	public sealed class SmsNotificationProducer : RabbitMqProducerBase<PriorityIntegrationEvent<SendSmsNotification>>
	{
		private readonly static string XMaxPriorityKey = "x-max-priority";
		private readonly static object XMaxPriorityVal = 3;

		public SmsNotificationProducer(
			IPooledObjectPolicy<IModel> pooledChannelPolicy,
			RabbitMqPubSubSettings settings,
			ILogger<SmsNotificationProducer> logger)
			: base(
				  pooledChannelPolicy,
				  settings,
				  logger,
				  new[] { (key: XMaxPriorityKey, val: XMaxPriorityVal) })
		{
		}

		protected override string AppId => "FintechIQ.ENS";

		/// <summary>
		/// Установить бозовые свойства по данным из события
		/// </summary>
		/// <param name="basicProperties">Базовые свойства</param>
		/// <param name="event">Интеграционное событие с приоритетом</param>
		protected override void SetBasicProperties(IBasicProperties basicProperties, PriorityIntegrationEvent<SendSmsNotification> @event)
		{
			basicProperties.Priority = @event.Priority;
			base.SetBasicProperties(basicProperties, @event);
		}
	}
}
