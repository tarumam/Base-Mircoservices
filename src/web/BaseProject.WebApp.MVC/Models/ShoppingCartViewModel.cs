using System;
using System.Collections.Generic;

namespace BaseProject.WebApp.MVC.Models
{
    public class ShoppingCartViewModel
    {
        public decimal TotalValue { get; set; }
        public List<ProductItemViewModel> Items { get; set; } = new List<ProductItemViewModel>();
    }
    public class ProductItemViewModel
    {
        public Guid ProductId { get; set; }
        public string Name { get; set; }
        public int Amount { get; set; }
        public decimal Value { get; set; }
        public string Image { get; set; }
    }
}