namespace Weelo.Domain.Models
{
    public class OwnerModel
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public byte[] Photo { get; set; }
        public DateTime BirthDate { get; set; }
    }
}
