namespace Weelo.API.Models
{
    public class PropertyTraceModel
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public decimal Tax { get; set; }

        public PropertyModel Property { get; set; }
    }
}
