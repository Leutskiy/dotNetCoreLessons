using System;
using System.Collections.Generic;

namespace EFCoreTutorial.Lesson_01.Domain
{
	/// <summary>
	/// Учитель
	/// </summary>
	public sealed class Teacher : DomainEntity
	{
		public Teacher(
			string name,
			School school) : this()
		{
			Name = name;
			School = school;

			school.HireATeacher(this);
		}

		private Teacher() : base()
		{
			Classes = new List<Class>();
		}

		public string Name { get; private set; }

		public School School { get; private set; }

		public ICollection<Class> Classes { get; private set; }

		public void BindToClass(Class @class)
		{
			Classes.Add(@class);
			@class.ChangeATeacher(this);
		}
	}
}