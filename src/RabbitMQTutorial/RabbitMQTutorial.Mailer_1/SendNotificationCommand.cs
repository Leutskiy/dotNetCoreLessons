using MediatR;
using System;

namespace RabbitMQTutorial.Mailer_1
{
	public sealed class SendNotificationCommand : IRequest<Unit>
	{
		public Guid NotificationId { get; set; }

		public string Message { get; set; }

		public string Recipient { get; set; }
	}
}
