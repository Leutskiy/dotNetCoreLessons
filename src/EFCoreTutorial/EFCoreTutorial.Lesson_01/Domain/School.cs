using System.Collections.Generic;

namespace EFCoreTutorial.Lesson_01.Domain
{
	/// <summary>
	/// Школа
	/// </summary>
	public sealed class School : DomainEntity
    {
		public School(
            string name,
            string city,
            string state) : this()
		{
			Name = name;
			City = city;
			State = state;
		}
        
        private School() : base()
		{
            Teachers = new List<Teacher>();
		}

        public string Name { get; private set; }

        public string City { get; private set; }

        public string State { get; private set; }

        public ICollection<Teacher> Teachers { get; private set; }

        public void HireATeacher(Teacher teacher)
		{
            Teachers.Add(teacher);
		}
    }
}
