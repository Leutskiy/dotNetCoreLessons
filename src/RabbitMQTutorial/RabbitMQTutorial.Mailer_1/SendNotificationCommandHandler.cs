using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace RabbitMQTutorial.Mailer_1
{
	public sealed class SendNotificationCommandHandler : IRequestHandler<SendNotificationCommand, Unit>
	{
		private readonly ILogger<SendNotificationCommandHandler> _logger;

		public SendNotificationCommandHandler(ILogger<SendNotificationCommandHandler> logger)
		{
			_logger = logger;
		}

		public Task<Unit> Handle(SendNotificationCommand request, CancellationToken cancellationToken)
		{
			_logger.LogInformation($"Send to {request.Recipient} the following message: {request.Message}");

			return Task.FromResult(Unit.Value);
		}
	}
}
