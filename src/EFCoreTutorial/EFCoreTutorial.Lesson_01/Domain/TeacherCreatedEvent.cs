using System;

namespace EFCoreTutorial.Lesson_01.Domain
{
    public sealed class TeacherCreatedEvent : IDomainEvent
    {
        public TeacherCreatedEvent(
            DateTime createdDate,
            string creator)
        {
            CreatedAt = createdDate;
            CreatedBy = creator;
        }

        private TeacherCreatedEvent() { }

        public DateTime CreatedAt { get; }

        public string CreatedBy { get; }
    }
}