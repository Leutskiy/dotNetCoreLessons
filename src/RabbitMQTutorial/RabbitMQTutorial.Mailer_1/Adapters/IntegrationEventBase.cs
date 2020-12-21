using System;
using static System.Guid;

namespace RabbitMQTutorial.Mailer_1.Adapters
{
	/// <summary>
	/// Базовый класс события интеграции
	/// </summary>
	public abstract class IntegrationEventBase
    {
        /// <summary>
        /// Конструктор без параметров
        /// </summary>
        public IntegrationEventBase()
        {
            // TODO: изменить наименование метода Generate
            Id = NewGuid();
            Timestamp = DateTimeOffset.UtcNow;
        }

        /// <summary>
        /// Идентификатор события
        /// </summary>
        public Guid Id { get; }

        /// <summary>
        /// Временная отметка возникновения события 
        /// в UTC c учетом часового пояса
        /// </summary>
        public DateTimeOffset Timestamp { get; }
    }
}