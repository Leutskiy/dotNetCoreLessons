using EFCoreTutorial.Lesson_01.Controllers.DTOs;
using EFCoreTutorial.Lesson_01.Domain;
using Microsoft.AspNetCore.Mvc;

namespace EFCoreTutorial.Lesson_01.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class StudentController : ControllerBase
	{
        [HttpGet("{id}")]
        public StudentDTO Get(int id)
        {
            return ToDTO(new Student
            {
                Id = id,
                Name = "Jim Bob",
                Class = new Class
                {
                    Id = 1,
                    Name = "Fifth Grade Class",
                    Teacher = new Teacher
                    {
                        Id = 1,
                        Name = "Mrs. Stricter",
                        School = new School
                        {
                            Id = 1,
                            Name = "School of Hard Knocks",
                            City = "Life",
                            State = "Madness"
                        }
                    }
                }
            });
        }

        private static StudentDTO ToDTO(Student student)
        {
            return new StudentDTO
            {
                Id = student.Id,
                Name = student.Name,
                ClassId = student.Class.Id,
                TeacherId = student.Class.Teacher.Id,
                SchoolId = student.Class.Teacher.School.Id
            };
        }
    }
}
