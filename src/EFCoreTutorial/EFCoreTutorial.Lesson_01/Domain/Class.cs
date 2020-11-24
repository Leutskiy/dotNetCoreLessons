using System.Collections.Generic;

namespace EFCoreTutorial.Lesson_01.Domain
{
	/// <summary>
	/// Класс
	/// </summary>
	public sealed class Class : DomainEntity
    {
		public Class(
            string name) : this()
		{
			Name = name;
			Teacher = null;
		}

		private Class() : base()
		{
            Students = new List<Student>();
		}

        public string Name { get; private set; }

        public Teacher Teacher { get; private set; }

        public ICollection<Student> Students { get; private set; }

        public void ChangeATeacher(Teacher teacher)
		{
            Teacher = teacher;
		}

        public void AddAStudent(Student student)
		{
            Students.Add(student);
		}
    }
}
