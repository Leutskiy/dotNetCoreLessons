using System;

namespace SwaggerTutorial.Lesson_01.Controllers
{
	/// <summary>
	/// Данные для отправки СМС уведомления
	/// </summary>
	public sealed class SmsNotificationDto
	{
		/// <summary>
		/// Идентификатор СМС
		/// </summary>
		public Guid SmsNotificationId { get; }

		/// <summary>
		/// Сообщение
		/// </summary>
		public string MessageText { get; }

		/// <summary>
		/// Номер телефона получателя
		/// </summary>
		public PhoneNumber PhoneNumber { get; }
	}
}