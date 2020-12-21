namespace RabbitMQTutorial.Mailer_1.Adapters
{
    /// <summary>
    /// Интерфейс производителя
    /// </summary>
    /// <typeparam name="TEvent">Событие</typeparam>
    public interface IProducer<in TEvent>
    {
        void Publish(TEvent @event);
    }
}
