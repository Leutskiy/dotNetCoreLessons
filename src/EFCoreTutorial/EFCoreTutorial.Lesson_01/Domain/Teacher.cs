using System;
using System.Collections.Generic;

namespace EFCoreTutorial.Lesson_01.Domain
{
	/// <summary>
	/// Учитель
	/// </summary>
	public sealed class Teacher : DomainEntity, IHasDomainEvents
	{
		private readonly ICollection<IDomainEvent> _domainEvents;

		public Teacher(
			string name) : this()
		{
			Name = name;
			Classes.Add(new Class($"Start_Initial_{DateTime.Now}", Id));

			_domainEvents.Add(new TeacherCreatedEvent(DateTime.Now, "leujo"));
		}

		private Teacher() : base()
		{
			Classes = new List<Class>();

			_domainEvents = new List<IDomainEvent>();
		}

		public string Name { get; private set; }

		public ICollection<Class> Classes { get; private set; }

        public ICollection<IDomainEvent> DomainEvents => _domainEvents;

        public void AddClass(Class @class)
		{
			Classes.Add(@class);
		}
	}
}