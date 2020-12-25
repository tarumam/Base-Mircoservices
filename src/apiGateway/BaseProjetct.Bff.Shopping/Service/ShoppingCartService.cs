using System;
using System.Net.Http;
using System.Threading.Tasks;
using BaseProject.Core.Communication;
using BaseProjetct.Bff.Shopping.Extensions;
using BaseProjetct.Bff.Shopping.Models;
using Microsoft.Extensions.Options;

namespace BaseProjetct.Bff.Shopping.Service
{
    public interface IShoppingCartService
    {
        Task<ShoppingCartDTO> GetShoppingCart();
        Task<ResponseResult> AddItemToShoppingCart(ShoppingCartItemDTO product);
        Task<ResponseResult> UpdateItemInShoppingCart(Guid productId, ShoppingCartItemDTO shoppingCart);
        Task<ResponseResult> RemoveItemFromShoppingCart(Guid productId);
    }

    public class ShoppingCartService : Service, IShoppingCartService
    {
        private readonly HttpClient _httpClient;

        public ShoppingCartService(HttpClient httpClient, IOptions<AppServicesSettings> settings)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(settings.Value.ShoppingCartUrl);
        }
        public async Task<ShoppingCartDTO> GetShoppingCart()
        {
            var response = await _httpClient.GetAsync("/shoppingCart/");

            HandleErrorsResponse(response);

            return await DeserializeObjetoResponse<ShoppingCartDTO>(response);
        }

        public async Task<ResponseResult> AddItemToShoppingCart(ShoppingCartItemDTO product)
        {
            var itemContent = GetContent(product);

            var response = await _httpClient.PostAsync("/shoppingCart/", itemContent);

            if (!HandleErrorsResponse(response)) return await DeserializeObjetoResponse<ResponseResult>(response);

            return ReturnOk();
        }

        public async Task<ResponseResult> RemoveItemFromShoppingCart(Guid productId)
        {
            var response = await _httpClient.DeleteAsync($"/shoppingCart/{productId}");

            if (!HandleErrorsResponse(response)) return await DeserializeObjetoResponse<ResponseResult>(response);

            return ReturnOk();
        }

        public async Task<ResponseResult> UpdateItemInShoppingCart(Guid productId, ShoppingCartItemDTO shoppingCartItem)
        {
            var itemContent = GetContent(shoppingCartItem);

            var response = await _httpClient.PutAsync($"/shoppingCart/{shoppingCartItem.ProductId}", itemContent);

            if (!HandleErrorsResponse(response)) return await DeserializeObjetoResponse<ResponseResult>(response);

            return ReturnOk();
        }
    }
}
