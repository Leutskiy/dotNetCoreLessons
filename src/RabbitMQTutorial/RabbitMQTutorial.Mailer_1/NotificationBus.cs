using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace RabbitMQTutorial.Mailer_1
{
	public sealed class NotificationBus
	{
		private readonly string _hostname;
		private readonly string _queueName;
		private readonly string _username;
		private readonly string _password;

		public NotificationBus(IOptions<RabbitMqConfiguration> rabbitMqOptions)
		{
			_hostname = rabbitMqOptions.Value.Hostname;
			_queueName = rabbitMqOptions.Value.QueueName;
			_username = rabbitMqOptions.Value.UserName;
			_password = rabbitMqOptions.Value.Password;
		}

		public void Publish(SendNotificationCommand sendNotificationCommand)
		{
			var factory = new ConnectionFactory()
			{
				HostName = _hostname,
				UserName = _username,
				Password = _password
			};

			using (var connection = factory.CreateConnection())
			using (var channel = connection.CreateModel())
			{
				channel.QueueDeclare(
					queue: _queueName,
					durable: false,
					exclusive: false,
					autoDelete: false,
					arguments: null);

				var json = JsonConvert.SerializeObject(sendNotificationCommand);
				var body = Encoding.UTF8.GetBytes(json);

				channel.BasicPublish(
					exchange: _queueName,
					routingKey: "",
					mandatory: false,
					basicProperties: null,
					body: body);
			}
		}
	}
}
