namespace RabbitMQTutorial.Mailer_1.Adapters
{
    /// <summary>
    /// Событие интеграции с RabbitMQ имеющее приоритет и полезную наргрузку
    /// </summary>
    /// <typeparam name="TPayload">Тип полезной нагрузки</typeparam>
    public class PriorityIntegrationEvent<TPayload> : IntegrationEvent<TPayload>
    {
        /// <summary>
        /// Конструктор с параметрами
        /// </summary>
        /// <param name="data">Полезная нагрузка</param>
        /// <param name="priority">Приоритет события</param>
        public PriorityIntegrationEvent(TPayload data, byte priority)
            : base(data)
        {
            Priority = priority;
        }

        /// <summary>
        /// Приоритет события
        /// </summary>
        public byte Priority { get; }
    }
}