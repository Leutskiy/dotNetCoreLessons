using System;

namespace EFCoreTutorial.Lesson_01.Domain
{
    /// <summary>
    /// Класс
    /// </summary>
    public sealed class Class : DomainEntity
    {
		public Class(
            string name,
            Guid teacherId) : this()
		{
			Name = name;
            TeacherId = teacherId;
		}

		private Class() : base()
		{
		}

        public string Name { get; private set; }

        public Guid TeacherId { get; private set; }
    }
}
