using System;
using System.Threading.Tasks;
using BaseProject.Core.Communication;
using BaseProject.WebApp.MVC.Models;

namespace BaseProject.WebApp.MVC.Services
{
    public interface IShoppingCartService
    {
        Task<ShoppingCartViewModel> GetShoppingCart();
        Task<ResponseResult> AddItemToShoppingCart(ProductItemViewModel produto);
        Task<ResponseResult> UpdateItemInShoppingCart(Guid produtoId, ProductItemViewModel produto);
        Task<ResponseResult> RemoveItemFromShoppingCart(Guid produtoId);
    }
}
