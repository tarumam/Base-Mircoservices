using BaseProject.Core.DomainObjects;

namespace BaseProject.Seller.API.Models
{
    public class SellerModel
    {
        public string DocRef { get; set; }
        public string DocType { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Neighbohood { get; set; }
        public string Number { get; set; }
        public string Reference { get; set; }
        public string WorkingTime { get; set; }
        public string Details { get; set; }
        public bool HasParking { get; set; }
        public string Image { get; set; }
    }
}
