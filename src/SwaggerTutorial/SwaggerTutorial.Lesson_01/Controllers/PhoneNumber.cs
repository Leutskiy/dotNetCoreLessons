using System;
using System.ComponentModel.DataAnnotations;

namespace SwaggerTutorial.Lesson_01.Controllers
{
	public sealed class PhoneNumber
	{
		private readonly string _value;

		private static readonly PhoneAttribute _phoneValidator;

		static PhoneNumber()
		{
			_phoneValidator = new PhoneAttribute();
		}

		public PhoneNumber(string phone)
		{
			if (!_phoneValidator.IsValid(phone))
			{
				throw new ArgumentException(nameof(phone), "{phone} is not valid phone");
			}

			_value = phone;
		}

		public string Value => _value;
	}
}