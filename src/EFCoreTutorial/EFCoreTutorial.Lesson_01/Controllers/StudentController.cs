using EFCoreTutorial.Lesson_01.Controllers.DTOs;
using EFCoreTutorial.Lesson_01.Domain;
using EFCoreTutorial.Lesson_01.Infrastructure.Database;
using Microsoft.AspNetCore.Mvc;

namespace EFCoreTutorial.Lesson_01.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class StudentController : ControllerBase
	{
        private readonly ApplicationContext _applicationContext;

		public StudentController(ApplicationContext applicationContext)
		{
			_applicationContext = applicationContext;
		}

        /// <summary>
        /// Получить информацию о студенте
        /// </summary>
        /// <param name="id">Идентификатор студента</param>
        /// <returns>Информация о студенте</returns>
        [HttpGet("{id}")]
        public StudentDTO Get(int id)
        {
            //return ToDTO(new Student
            //{
            //    Id = id,
            //    Name = "Jim Bob",
            //    Class = new Class
            //    {
            //        Id = 1,
            //        Name = "Fifth Grade Class",
            //        Teacher = new Teacher
            //        {
            //            Id = 1,
            //            Name = "Mrs. Stricter",
            //            School = new School
            //            {
            //                Id = 1,
            //                Name = "School of Hard Knocks",
            //                City = "Life",
            //                State = "Madness"
            //            }
            //        }
            //    }
            //});

            return null;
        }

        //private static StudentDTO ToDTO(Student student)
        //{
        //    return new StudentDTO
        //    {
        //        Id = student.Id,
        //        Name = student.Name,
        //        ClassId = student.Class.Id,
        //        TeacherId = student.Class.Teacher.Id,
        //        SchoolId = student.Class.Teacher.School.Id
        //    };
        //}

        /// <summary>
        /// Проведение теста с EF Core
        /// </summary>
        [HttpPost("test")]
        public void Test()
		{
            var teacher = new Teacher("Alice");
            _applicationContext.Teachers.Add(teacher);

            //_applicationContext.Classes.AddRange(@class1, @class2);
            //teacher.AddClass(@class1);

            //teacher.AddClass(new Class("SuperClass", teacher.Id));

            _applicationContext.SaveChanges();


            var @class1 = new Class("SuperClass", teacher.Id);
            var @class2 = new Class("MegaClass", teacher.Id);
            //teacher.AddClass(new Class("MegaClass", teacher.Id));

            teacher.AddClass(@class1);
            teacher.AddClass(@class2);
            _applicationContext.SaveChanges();
        }
    }
}
