using System;
using System.ComponentModel.DataAnnotations;

namespace BaseProject.Catalog.API.Models
{
    public class PriceModel
    {
        public Guid? Id { get; set; }
        
        [Required(ErrorMessage = "Informe o produto")]
        public Guid ProductId { get; set; }
        
        [Required(ErrorMessage = "Informe o Vendedor")]
        public Guid SellerId { get; set; }
        
        [Required(ErrorMessage = "Informe o Valor")]
        public decimal Value { get; set; }
        public bool Active { get; set; }
    }
}
