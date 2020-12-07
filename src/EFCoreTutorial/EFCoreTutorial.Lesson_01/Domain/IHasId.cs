using System;

namespace EFCoreTutorial.Lesson_01.Domain
{
    public interface IHasId
    {
        Guid Id { get; }
    }
}