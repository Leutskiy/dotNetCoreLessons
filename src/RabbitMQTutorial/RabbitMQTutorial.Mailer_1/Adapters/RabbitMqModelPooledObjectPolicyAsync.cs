using Microsoft.Extensions.ObjectPool;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace RabbitMQTutorial.Mailer_1.Adapters
{
    /// <summary>
    /// Политика управления жизненным циклом каналов RabbitMq
    /// </summary>
	public class RabbitMqModelPooledObjectPolicyAsync : IPooledObjectPolicy<IModel>
    {
        private readonly IConnection _connection;

        public RabbitMqModelPooledObjectPolicyAsync(RabbitMqConnectionSettings options)
        {
            _connection = GetConnection(options);
        }

        private IConnection GetConnection(RabbitMqConnectionSettings connectionSettings)
        {
            var factory = new ConnectionFactory()
            {
                VirtualHost = connectionSettings.VirtualHost,
                HostName = connectionSettings.HostName,
                UserName = connectionSettings.UserName,
                Password = connectionSettings.Password,
                Port = connectionSettings.Port,
                DispatchConsumersAsync = true
            };

            return factory.CreateConnection();
        }

        public IModel Create()
        {
            return _connection.CreateModel();
        }

        public bool Return(IModel obj)
        {
            if (obj.IsOpen)
            {
                return true;
            }
            else
            {
                obj.Dispose();
                return false;
            }
        }
    }
}
