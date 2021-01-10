using MediatR;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RabbitMQTutorial.Mailer_1.Features.SendSms
{
	public sealed partial class SendSms
	{
		// добавить возвращение рещультаты команды
		public sealed class CommandHadler : IRequestHandler<Command, CommandResult>
		{
			private readonly ILogger<CommandHadler> _logger;

			public CommandHadler(ILogger<CommandHadler> logger)
			{
				_logger = logger;
			}

			public async Task<CommandResult> Handle(Command command, CancellationToken cancellationToken)
			{
				await Task.Delay(500, cancellationToken);

				_logger.LogInformation($"Id: {command.Notification.NotificationId} Text: {command.Notification.MessageText}");

				await Task.Delay(2500, cancellationToken);

				return new CommandResult
				{
					NotificationId = command.Notification.NotificationId,
					IsSuccess = true
				};
			}
		}
	}
}
