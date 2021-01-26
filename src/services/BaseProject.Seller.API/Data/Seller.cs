using BaseProject.Core.DomainObjects;

namespace BaseProject.Seller.API.Data
{
    public class Seller : Entity, IAggregateRoot
    {
        public Seller() { }

        public Seller(string name, string address, string neighbohood, string number, string reference, string workingTime, string details, string image, bool hasParking)
        {
            Name = name;
            Address = address;
            Neighbohood = neighbohood;
            Number = number;
            Reference = reference;
            WorkingTime = workingTime;
            Details = details;
            Image = Image;
            HasParking = HasParking;
        }
        public string DocRef { get; private set; }
        public string DocType { get; private set; }
        public string Name { get; private set; }
        public string Address { get; private set; }
        public string Neighbohood { get; private set; }
        public string Number { get; private set; }
        public string City { get; private set; }
        public string Reference { get; private set; }
        public string WorkingTime { get; private set; }
        public string Details { get; private set; }
        public string Image { get; private set; }
        public bool HasParking { get; private set; }
        public bool HasDelivery { get; private set; }
    }
}
