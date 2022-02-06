using Weelo.Domain.Interfaces.Entities;

namespace Weelo.Domain.Entities
{
    public class Owner : IEntity<long>, IAudit
    {
        public long Id { get; set; }
        public string DocumentNumber { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public byte[]? Photo { get; set; }
        public DateTime BirthDay { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public virtual ICollection<Property> Properties { get; set; } = new HashSet<Property>();
    }
}
