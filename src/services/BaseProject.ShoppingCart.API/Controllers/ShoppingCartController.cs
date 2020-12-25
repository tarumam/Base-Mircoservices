using System;
using System.Linq;
using System.Threading.Tasks;
using BaseProject.ShoppingCart.API.Data;
using BaseProject.ShoppingCart.API.Model;
using BaseProject.WebAPI.Core.Controllers;
using BaseProject.WebAPI.Core.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BaseProject.ShoppingCart.API.Controllers
{
    [Authorize]
    public class ShoppingCartController : MainController
    {
        private readonly IAspNetUser _user;
        private readonly ShoppingCartContext _context;
        public ShoppingCartController(IAspNetUser user, ShoppingCartContext context)
        {
            _context = context;
            _user = user;
        }

        [HttpGet("shoppingCart")]
        public async Task<ShoppingCartClient> GetShoppingCart()
        {
            return await GetClientShoppingCart() ?? new ShoppingCartClient();
        }

        [HttpPost("shoppingCart")]
        public async Task<IActionResult> AddShoppingCartItem(ShoppingCartItem item)
        {
            var shoppingCart = await GetClientShoppingCart();

            if (shoppingCart == null)
                ManipulateNewShoppingCart(item);
            else
                ManipulateExistentShoppingCart(shoppingCart, item);

            if (!IsOperationValid()) return CustomResponse();

            await SaveContext();

            return CustomResponse();
        }

        [HttpPut("shoppingCart/{productId}")]
        public async Task<IActionResult> UpdateShoppingCartItem(Guid productId, ShoppingCartItem item)
        {
            var shoppingCart = await GetClientShoppingCart();
            var shoppingCartItem = await GetValidShoppingCartItem(productId, shoppingCart, item);
            if (shoppingCartItem == null) return CustomResponse();

            shoppingCart.UpdateUnities(shoppingCartItem, item.Amount);

            ValidateShoppingCart(shoppingCart);
            if (!IsOperationValid()) return CustomResponse();

            _context.ShoppingCartItems.Update(shoppingCartItem);
            _context.ShoppingCartClients.Update(shoppingCart);

            await SaveContext();
            return CustomResponse();
        }

        [HttpDelete("shoppingCart/{productId}")]
        public async Task<IActionResult> RemoveShoppingCartItem(Guid productId)
        {
            var shoppingCart = await GetClientShoppingCart();
            var shoppingCartItem = await GetValidShoppingCartItem(productId, shoppingCart);
            if (shoppingCartItem == null) return CustomResponse();

            ValidateShoppingCart(shoppingCart);
            if (!IsOperationValid()) return CustomResponse();

            shoppingCart.RemoveItem(shoppingCartItem);

            _context.ShoppingCartItems.Remove(shoppingCartItem);
            _context.ShoppingCartClients.Update(shoppingCart);

            await SaveContext();
            return CustomResponse();
        }
        private async Task SaveContext()
        {
            var result = await _context.SaveChangesAsync();
            if (result <= 0) AddProcessingError("Não fo possível persistir os daods no banco");
        }
        private async Task<ShoppingCartClient> GetClientShoppingCart()
        {
            return await _context.ShoppingCartClients
                            .Include(c => c.Items)
                            .FirstOrDefaultAsync(c => c.ClientId == _user.GetUserId());
        }
        private void ManipulateExistentShoppingCart(ShoppingCartClient shoppingCart, ShoppingCartItem item)
        {
            var existentItem = shoppingCart.IsItemExistentInShoppingCart(item);
            shoppingCart.AddItem(item);

            ValidateShoppingCart(shoppingCart);

            if (existentItem)
            {
                _context.ShoppingCartItems.Update(shoppingCart.GetProductById(item.ProductId));
            }
            else
            {
                _context.ShoppingCartItems.Add(item);
            }
            _context.ShoppingCartClients.Update(shoppingCart);
        }
        private void ManipulateNewShoppingCart(ShoppingCartItem item)
        {
            var shoppingCart = new ShoppingCartClient(_user.GetUserId());
            shoppingCart.AddItem(item);

            ValidateShoppingCart(shoppingCart);

            _context.ShoppingCartClients.Add(shoppingCart);
        }
        private async Task<ShoppingCartItem> GetValidShoppingCartItem(Guid productId, ShoppingCartClient shoppingCart, ShoppingCartItem item = null)
        {
            if (item != null && productId != item.ProductId)
            {
                AddProcessingError("O item não corresponde ao informado");
                return null;
            }

            if (shoppingCart == null)
            {
                AddProcessingError("Carrinho não encontrado");
                return null;
            }

            var shoppingCartItem = await _context.ShoppingCartItems
                .FirstOrDefaultAsync(i => i.ShoppingCartId == shoppingCart.Id && i.ProductId == productId);

            if (shoppingCartItem == null || !shoppingCart.IsItemExistentInShoppingCart(shoppingCartItem))
            {
                AddProcessingError("O item não está no carrinho");
                return null;
            }
            return shoppingCartItem;
        }
        private bool ValidateShoppingCart(ShoppingCartClient shoppingCart)
        {
            if (shoppingCart.IsValid()) return true;

            shoppingCart.ValidationResult.Errors.ToList().ForEach(e => AddProcessingError(e.ErrorMessage));
            return false;
        }
    }
}
