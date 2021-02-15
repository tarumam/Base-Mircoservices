using System;
using System.ComponentModel.DataAnnotations;
using BaseProject.Core.Utils;

namespace BaseProject.Catalog.API.Models
{
    public class PriceModel
    {
        public Guid? Id { get; set; }

        [RequiredIf(nameof(Barcode), null, ErrorMessage = "Informe o produto")]
        public Guid? ProductId { get; set; }

        [RequiredIf(nameof(ProductId), null, ErrorMessage = "Informe o produto")]
        public string Barcode { get; set; }

        [Required(ErrorMessage = "Informe o Vendedor")]
        public Guid SellerId { get; set; }

        [Required(ErrorMessage = "Informe o Valor")]
        public decimal Value { get; set; }
        public bool Active { get; set; }
    }
}
