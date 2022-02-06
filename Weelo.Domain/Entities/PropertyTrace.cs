using Weelo.Domain.Interfaces.Entities;

namespace Weelo.Domain.Entities
{
    public class PropertyTrace : IEntity<long>
    {
        public long Id { get; set; }
        public DateTime DateSale { get; set; }
        public string Name { get; set; }
        public decimal Value { get; set; }
        public decimal Tax { get; set; }
        public long IdProperty { get; set; }

        public virtual Property Property { get; set; }
    }
}
