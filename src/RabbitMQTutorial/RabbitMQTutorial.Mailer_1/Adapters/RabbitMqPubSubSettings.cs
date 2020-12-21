namespace RabbitMQTutorial.Mailer_1.Adapters
{
    /// <summary>
    /// Настройки производителя/потребителя RabbitMq
    /// </summary>
    public class RabbitMqPubSubSettings
    {
        /// <summary>
        /// Наименование очереди RabbitMQ
        /// </summary>
        public string QueueName { get; set; }

        /// <summary>
        /// Наименование точки обмена (или обменника) RabbitMQ
        /// </summary>
        public string ExchangeName { get; set; }

        /// <summary>
        /// Наименование ключа маршрутизации сообщений
        /// </summary>
        public string RouteKeyName { get; set; }
    }
}
