using Weelo.Domain.Interfaces.Entities;

namespace Weelo.Domain.Entities
{
    public class PropertyImage : IEntity<long>, IAudit
    {
        public long Id { get; set; }
        public long IdProperty { get; set; }
        public byte[] File { get; set; }
        public bool Enabled { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public virtual Property Property { get; set; }
    }
}
