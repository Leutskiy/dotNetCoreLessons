namespace EFCoreTutorial.Lesson_01.Domain
{
	/// <summary>
	/// Студент
	/// </summary>
	public sealed class Student : DomainEntity
	{
		public Student(
			string name,
			Class @class) : this()
		{
			Name = name;
			Class = @class;
		}

		private Student() : base()
		{

		}

		public string Name { get; private set; }

		public Class Class { get; private set; }

		public void GoToAClass(Class @class)
		{
			Class = @class;
		}
	}
}