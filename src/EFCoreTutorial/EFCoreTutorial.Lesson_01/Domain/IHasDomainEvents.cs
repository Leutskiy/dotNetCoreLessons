using System.Collections.Generic;

namespace EFCoreTutorial.Lesson_01.Domain
{
    public interface IHasDomainEvents
    {
        ICollection<IDomainEvent> DomainEvents { get; }
    }
}