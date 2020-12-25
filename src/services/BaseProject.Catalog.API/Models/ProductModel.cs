using System;
using System.ComponentModel.DataAnnotations;

namespace BaseProject.Catalog.API.Models
{
    public class ProductModel
    {
        public Guid? Id { get; set; }
        public string Barcode { get; set; }

        [Required(ErrorMessage = "Informe o nome do produto")]
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Active { get; set; }
        public string Image { get; set; }

    }
}
