namespace RabbitMQTutorial.Mailer_1.Adapters
{
	/// <summary>
	/// Событие интеграции с полезной нагрузкой
	/// </summary>
	/// <typeparam name="TPayload">Тип полезной нагрузки</typeparam>
	public class IntegrationEvent<TPayload> : IntegrationEventBase
    {
        /// <summary>
        /// Конструктор с параметрами
        /// </summary>
        /// <param name="data">Полезная нагрузка</param>
        public IntegrationEvent(TPayload data)
            : base()
        {
            Data = data;
        }

        /// <summary>
        /// Полезная нагрузка
        /// </summary>
        public TPayload Data { get; }
    }
}