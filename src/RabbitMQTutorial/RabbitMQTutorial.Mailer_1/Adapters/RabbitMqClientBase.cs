using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.ObjectPool;
using RabbitMQ.Client;

using static RabbitMQ.Client.ExchangeType;

namespace RabbitMQTutorial.Mailer_1.Adapters
{
	// TODO: добавить init-setter когда перейдем на C# 9.0

	/// <summary>
	/// Базовый класс клиента для интеграции с RabbitMQ
	/// </summary>
	public abstract class RabbitMqClientBase
    {
        private const string RetryExchange = "fintechiq.retry.exchange";
        private const string RetryQueue = "fintechiq.retry.queue";

        /// <summary>
        /// Констурктор с параметрами
        /// </summary>
        /// <param name="pooledChannelPolicy">Политика управления каналами в пуле</param>
        /// <param name="options">Общии настройки подключения для publisher/consumer</param>
        /// <param name="args"></param>
        protected RabbitMqClientBase(
            IPooledObjectPolicy<IModel> pooledChannelPolicy,
            RabbitMqPubSubSettings options,
            (string key, object val)[]? args = default)
            : this()
        {
            // TODO: прорефакторить магические числа
            ChannelPool = new DefaultObjectPool<IModel>(pooledChannelPolicy, Environment.ProcessorCount * 2);

            ExchangeName = options.ExchangeName;
            QueueName = options.QueueName;
            RouteKeyName = options.RouteKeyName;

            SetupRabbitMqQueue(args);
        }

        /// <summary>
        /// Конструктор без параметров
        /// </summary>
        private RabbitMqClientBase()
		{
            Arguments = new Dictionary<string, object>();
        }

        /// <summary>
        /// Наименование точки обмена (или обменника) RabbitMQ
        /// </summary>
        protected string ExchangeName { get; }

        /// <summary>
        /// Наименование очереди RabbitMQ
        /// </summary>
        protected string QueueName { get; }

        /// <summary>
        /// Наименование ключа маршрутизации сообщений
        /// </summary>
        protected string RouteKeyName { get; }

        /// <summary>
        /// Аргументы объявляемой очереди RabbitMQ
        /// </summary>
        protected Dictionary<string, object> Arguments { get; }

        /// <summary>
        /// Пул каналов RabbitMq
        /// </summary>
        protected DefaultObjectPool<IModel> ChannelPool { get; }

        /// <summary>
        /// Добавить новые параметры конфигурирования очереди
        /// </summary>
        /// <param name="args">Дополнительные аргументы</param>
        private void AddArguments((string key, object val)[] args)
        {
			foreach (var arg in args)
			{
                Arguments.Add(arg.key, arg.val);
            }
        }

        /// <summary>
        /// Создать подключение и канал к RabbitMQ
        /// </summary>
        private void SetupRabbitMqQueue((string key, object val)[]? args)
        {
            if (args != null)
			{
                AddArguments(args);
                Arguments.Add("x-dead-letter-exchange", RetryExchange);
                Arguments.Add("x-dead-letter-routing-key", RetryQueue);
            }
                
            var channel = ChannelPool.Get();

            try
			{
                channel.ExchangeDeclare(exchange: ExchangeName, type: Direct, durable: true, autoDelete: false);
                channel.QueueDeclare(queue: QueueName, durable: true, exclusive: false, autoDelete: false, arguments: Arguments);
                channel.QueueBind(queue: QueueName, exchange: ExchangeName, routingKey: RouteKeyName);

                channel.ExchangeDeclare(exchange: RetryExchange, type: Direct, durable: true, autoDelete: false);
                channel.QueueDeclare
                (
                    queue: RetryQueue, durable: true, exclusive: false, autoDelete: false,
                    arguments: new Dictionary<string, object> {
                        { "x-dead-letter-exchange", ExchangeName },
                        { "x-dead-letter-routing-key", RouteKeyName },
                        { "x-message-ttl", 30000 }, // 30 секунд
                    }
                );
                channel.QueueBind(RetryQueue, RetryExchange, RetryQueue);
            }
			finally
			{
                ChannelPool.Return(channel);
			}
        }
    }
}
