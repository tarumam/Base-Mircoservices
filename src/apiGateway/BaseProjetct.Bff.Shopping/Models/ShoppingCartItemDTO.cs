using System;

namespace BaseProjetct.Bff.Shopping.Models
{
    public class ShoppingCartItemDTO
    {
        public Guid ProductId { get; set; }
        public string Name { get; set; }
        public decimal Value { get; set; }
        public string Image { get; set; }
        public int Amount { get; set; }
    }
}
