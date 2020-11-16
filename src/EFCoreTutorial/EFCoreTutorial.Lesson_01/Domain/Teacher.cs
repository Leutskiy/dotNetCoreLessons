using System.Collections.Generic;

namespace EFCoreTutorial.Lesson_01.Domain
{
	public class Teacher
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public School School { get; set; }

		public ICollection<Class> Classes { get; set; }
	}
}