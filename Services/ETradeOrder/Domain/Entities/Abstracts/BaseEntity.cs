

namespace Domain.Entities.Abstracts
{
    public abstract class BaseEntity
    {
        public Guid Id { get; private set; }
        public virtual DateTime CreatedAt { get; set; }
        public virtual DateTime UpdatedAt { get; set; }
    }
}