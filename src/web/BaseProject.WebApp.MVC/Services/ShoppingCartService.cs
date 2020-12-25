using System;
using System.Net.Http;
using System.Threading.Tasks;
using BaseProject.Core.Communication;
using BaseProject.WebApp.MVC.Extensions;
using BaseProject.WebApp.MVC.Models;
using Microsoft.Extensions.Options;

namespace BaseProject.WebApp.MVC.Services
{
    public class ShoppingCartService : Service, IShoppingCartService
    {
        private readonly HttpClient _httpClient;

        public ShoppingCartService(HttpClient httpClient, IOptions<AppSettings> settings)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(settings.Value.ShoppingCartUrl);
        }

        public async Task<ShoppingCartViewModel> GetShoppingCart()
        {
            var response = await _httpClient.GetAsync("/shoppingCart/");

            HandleErrorsResponse(response);

            return await DeserializeObjetoResponse<ShoppingCartViewModel>(response);
        }

        public async Task<ResponseResult> AddItemToShoppingCart(ProductItemViewModel produto)
        {
            var itemContent = GetContent(produto);

            var response = await _httpClient.PostAsync("/shoppingCart/", itemContent);

            if (!HandleErrorsResponse(response)) return await DeserializeObjetoResponse<ResponseResult>(response);

            return ReturnOk();
        }

        public async Task<ResponseResult> UpdateItemInShoppingCart(Guid produtoId, ProductItemViewModel produto)
        {
            var itemContent = GetContent(produto);

            var response = await _httpClient.PutAsync($"/shoppingCart/{produto.ProductId}", itemContent);

            if (!HandleErrorsResponse(response)) return await DeserializeObjetoResponse<ResponseResult>(response);

            return ReturnOk();
        }

        public async Task<ResponseResult> RemoveItemFromShoppingCart(Guid produtoId)
        {
            var response = await _httpClient.DeleteAsync($"/shoppingCart/{produtoId}");

            if (!HandleErrorsResponse(response)) return await DeserializeObjetoResponse<ResponseResult>(response);

            return ReturnOk();
        }
    }
}
