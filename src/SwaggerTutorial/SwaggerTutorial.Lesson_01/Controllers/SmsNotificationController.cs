using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace SwaggerTutorial.Lesson_01.Controllers
{
	[Route("api/v1/notification/sms")]
	[ApiController]
	public sealed class SmsNotificationController : ControllerBase
	{
		private readonly ILogger<SmsNotificationController> _logger;

		public SmsNotificationController(ILogger<SmsNotificationController> logger)
		{
			_logger = logger;
		}

		public async Task SendSmsNotification(SmsNotificationDto smsNotificationDto)
		{
			_logger.LogInformation($"Send the following sms: id = {smsNotificationDto}, text = {smsNotificationDto}");
			await Task.Delay(1);
		}
	}
}
