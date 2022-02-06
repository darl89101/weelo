namespace Weelo.API.Models
{
    public class PropertyModel
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public decimal Price { get; set; }
        public int Year { get; set; }

        public OwnerModel Owner { get; set; }
    }
}
