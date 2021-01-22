using System;

namespace BaseProject.Catalog.API.Models
{

    public class CatalogDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string PriceRange { get; set; }
        public string MainImage { get; set; }
        public string Barcode { get; set; }
    }
}
