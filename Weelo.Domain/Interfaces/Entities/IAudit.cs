using System;

namespace Weelo.Domain.Interfaces.Entities
{
    public interface IAudit
    {
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
