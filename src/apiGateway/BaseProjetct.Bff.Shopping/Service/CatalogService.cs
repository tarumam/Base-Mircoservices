using System;
using System.Net.Http;
using System.Threading.Tasks;
using BaseProjetct.Bff.Shopping.Extensions;
using BaseProjetct.Bff.Shopping.Models;
using Microsoft.Extensions.Options;

namespace BaseProjetct.Bff.Shopping.Service
{
    public interface ICatalogService
    {
        public Task<ProductItemDTO> GetById(Guid id);
    }
    public class CatalogService : Service, ICatalogService
    {
        private readonly HttpClient _httpClient;

        public CatalogService(HttpClient httpClient, IOptions<AppServicesSettings> settings)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(settings.Value.ShoppingCartUrl);
        }

        public async Task<ProductItemDTO> GetById(Guid id)
        {
            var response = await _httpClient.GetAsync($"/catalog/products/{id}");
            HandleErrorsResponse(response);

            return await DeserializeObjetoResponse<ProductItemDTO>(response);
        }
    }
}
