using System;
using System.Linq;
using System.Threading.Tasks;
using BaseProject.WebAPI.Core.Controllers;
using BaseProjetct.Bff.Shopping.Models;
using BaseProjetct.Bff.Shopping.Service;
using Microsoft.AspNetCore.Mvc;

namespace BaseProjetct.Bff.Shopping.Controllers
{
    public class ShoppingController : MainController
    {
        private readonly IShoppingCartService _shoppingCartSvc;
        private readonly ICatalogService _catalogSvc;

        public ShoppingController(IShoppingCartService shoppingCartSvc, ICatalogService catalogSvc)
        {
            _shoppingCartSvc = shoppingCartSvc;
            _catalogSvc = catalogSvc;
        }

        [HttpGet, Route("shopping/shoppingCart")]
        public async Task<IActionResult> Index()
        {
            return CustomResponse(await _shoppingCartSvc.GetShoppingCart());
        }

        [HttpGet, Route("shopping/shopping-cart-amount")]
        public async Task<int> GetShoppingCartAmount()
        {
            var shoppingCart = await _shoppingCartSvc.GetShoppingCart();
            return shoppingCart?.Items.Sum(a => a.Amount) ?? 0;
        }

        [HttpPost, Route("shopping/shoppingcart/items")]
        public async Task<IActionResult> AddItemToShoppingCart(ShoppingCartItemDTO productItem)
        {
            var product = await _catalogSvc.GetById(productItem.ProductId);

            await ValidateItemInShoppingCart(product, productItem.Amount);
            if (!IsOperationValid()) return CustomResponse();

            productItem.Name = product.Name;
            productItem.Value = product.Value;
            productItem.Image = product.Image;

            var resp = await _shoppingCartSvc.AddItemToShoppingCart(productItem);

            return CustomResponse(resp);
        }

        [HttpPut, Route("shopping/shoppingcart/items/{productId}")]
        public async Task<IActionResult> UpdateShoppingCartItem(Guid productId, ShoppingCartItemDTO productItem)
        {
            var product = await _catalogSvc.GetById(productItem.ProductId);

            await ValidateItemInShoppingCart(product, productItem.Amount);
            if (!IsOperationValid()) return CustomResponse();

            var response = await _shoppingCartSvc.UpdateItemInShoppingCart(productId, productItem);
            return CustomResponse(response);
        }

        [HttpDelete, Route("shopping/shoppingcart/items/{productId}")]
        public async Task<IActionResult> RemoveShoppingCartItem(Guid productId)
        {
            var product = await _catalogSvc.GetById(productId);

            if(product == null)
            {
                AddProcessingError("Produto não existe");
                return CustomResponse();
            }
            var response = await _shoppingCartSvc.RemoveItemFromShoppingCart(productId);

            return CustomResponse(response);
        }

        private async Task ValidateItemInShoppingCart(ProductItemDTO product, int amount)
        {
            if (product == null) AddProcessingError("Produto intexistente");
            if (amount < 1) AddProcessingError($"A quantidade mínima do produto {product.Name} é 1");

            var shoppingCart = await _shoppingCartSvc.GetShoppingCart();
            var shoppingCartItem = shoppingCart.Items.FirstOrDefault(p => p.ProductId == product.Id);
        }
    }
}
