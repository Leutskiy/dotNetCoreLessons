namespace RabbitMQTutorial.Mailer_1.Adapters
{
    /// <summary>
    /// Настройки подключения к RabbitMq
    /// </summary>
	public sealed class RabbitMqConnectionSettings
    {
        /// <summary>
        /// Виртуальный хост
        /// </summary>
        public string VirtualHost { get; set; }
        
        /// <summary>
        /// Наименование хоста
        /// </summary>
        public string HostName { get; set; }

        /// <summary>
        /// Имя пользователя
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Пароль
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Порт
        /// </summary>
        public int Port { get; set; }
    }
}
