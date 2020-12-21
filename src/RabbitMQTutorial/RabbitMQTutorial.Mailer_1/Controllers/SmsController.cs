using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RabbitMQTutorial.Mailer_1.Adapters;
using System;
using System.Collections.Generic;
using RabbitMQTutorial.Mailer_1.Features.SendSms;

using SendSmsNotification = RabbitMQTutorial.Mailer_1.Features.SendSms.SendSms.Notification;

using static System.Guid;
using static System.Convert;

namespace RabbitMQTutorial.Mailer_1.Controllers
{
	[Route("api/v1/sms")]
	[ApiController]
	public class SmsController : ControllerBase
	{
		private static readonly Random _random = new Random();

		private readonly ILogger<SmsController> _logger;
		private readonly IProducer<PriorityIntegrationEvent<SendSmsNotification>> _priorityQueue;

		public SmsController(
			IProducer<PriorityIntegrationEvent<SendSmsNotification>> priorityQueue,
			ILogger<SmsController> logger)
		{
			_priorityQueue = priorityQueue;
			_logger = logger;
		}

		[HttpPost]
		[Route("send/bulk/{countMessages:int}")]
		public SmsNotificationResult SendSmsInBulk(int countMessages, [FromBody] SmsNotificationDto[] smsNotificationDtos)
		{
			smsNotificationDtos = GeneratemsNotificationDtos(smsNotificationDtos, countMessages);

			var ids = new List<Guid>();
			foreach (var smsNotificationDto in smsNotificationDtos)
			{
				var @id = NewGuid();
				var @text = smsNotificationDto.MessageText;
				var @priority = _random.Next(1, 4);

				var @command = new SendSmsNotification
				{
					NotificationId = @id,
					MessageText = $"> message test {@id}: priority={@priority} text={@text}"
				};

				var @event = new PriorityIntegrationEvent<SendSmsNotification>(@command, ToByte(@priority));

				_logger.LogInformation($"pushing {@id}");
				_priorityQueue.Publish(@event);
				_logger.LogInformation($"pushed");

				ids.Add(@id);
			}

			return new SmsNotificationResult
			{
				Ids = ids.ToArray(),
				Status = Status.InQueue,
				Message = "Нотификация в очереди на обработку"
			};
		}

		private SmsNotificationDto[] GeneratemsNotificationDtos(SmsNotificationDto[] smsNotificationDtos, int countMessages)
		{
			var dtos = new List<SmsNotificationDto>();
			dtos.AddRange(smsNotificationDtos);

			for (int i = 0; i < countMessages; i++)
			{
				var number = _random.Next(0, countMessages*100);
				var dto = new SmsNotificationDto
				{
					MessageText = $"text_{number}"
				};

				dtos.Add(dto);
			}

			return dtos.ToArray();
		}
	}

	public sealed class SmsNotificationResult
	{
		public Guid[] Ids { get; set; }

		public Status Status { get; set; }

		public string Message { get; set; }
	}
}
