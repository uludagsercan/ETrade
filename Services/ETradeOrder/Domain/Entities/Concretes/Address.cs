
namespace Domain.Entities.Concretes
{
    public class Address
    {
        public string Province { get; private set; } = null!;
        public string District { get; private set; }=null!;
        public string Street { get; private set; }=null!;
        public string ZipCode { get; private set; }=null!;
        public string Line { get; private set; }=null!;
    }
}