namespace RabbitMQTutorial.Mailer_1.Features.SendSms
{
	public enum Status
	{
		InQueue = 1,
		SentToOperator = 2,
		Delivered = 3,
		Error = 4
	}
}
