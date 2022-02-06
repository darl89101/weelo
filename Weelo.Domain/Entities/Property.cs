using Weelo.Domain.Interfaces.Entities;

namespace Weelo.Domain.Entities
{
    public class Property : IEntity<long>, IAudit
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public decimal Price { get; set; }
        public string CodeInternal { get; set; }
        public int Year { get; set; }
        public long IdOwner { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public virtual Owner Owner { get; set; }
        public virtual ICollection<PropertyTrace> PropertyTraces { get; set; } = new HashSet<PropertyTrace>();
        public virtual ICollection<PropertyImage> PropertyImages { get; set; } = new HashSet<PropertyImage>();
    }
}
