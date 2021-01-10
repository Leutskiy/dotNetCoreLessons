using MediatR;
using System;

namespace RabbitMQTutorial.Mailer_1.Features.SendSms
{
	public sealed partial class SendSms
	{
		public sealed class CommandResult
		{
			public Guid NotificationId { get; set; }

			public bool IsSuccess { get; set; }
		}

		public sealed class Notification
		{
			public Guid NotificationId { get; set; }

			public string MessageText { get; set; }
		}

		// добавить возвращение рещультаты команды
		public sealed class Command : IRequest<CommandResult>
		{
			public Notification Notification { get; set; }
		}
	}
}
