using System;

namespace BaseProjetct.Bff.Shopping.Models
{
    public class ProductItemDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Value { get; set; }
        public string Image { get; set; }
    }
}
