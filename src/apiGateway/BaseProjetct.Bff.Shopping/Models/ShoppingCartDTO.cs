using System.Collections.Generic;

namespace BaseProjetct.Bff.Shopping.Models
{
    public class ShoppingCartDTO
    {
        public decimal TotalValue { get; set; }
        public decimal Discount { get; set; }
        public List<ShoppingCartItemDTO> Items { get; set; } = new List<ShoppingCartItemDTO>();
    }
}
