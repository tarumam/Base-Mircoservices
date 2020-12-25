using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using BaseProject.WebApp.MVC.Extensions;
using BaseProject.WebApp.MVC.Models;
using Microsoft.Extensions.Options;

namespace BaseProject.WebApp.MVC.Services
{
    public class CatalogService : Service, ICatalogService
    {
        private readonly HttpClient _httpClient;

        public CatalogService(HttpClient httpClient,
            IOptions<AppSettings> settings)
        {
            httpClient.BaseAddress = new Uri(settings.Value.CatalogUrl);

            _httpClient = httpClient;
        }

        public async Task<ProductViewModel> GetById(Guid id)
        {
            var response = await _httpClient.GetAsync($"/catalog/produtcts/{id}");

            HandleErrorsResponse(response);

            return await DeserializeObjetoResponse<ProductViewModel>(response);
        }

        public async Task<IEnumerable<ProductViewModel>> GetAll()
        {
            var response = await _httpClient.GetAsync("/catalog/products/");

            HandleErrorsResponse(response);

            return await DeserializeObjetoResponse<IEnumerable<ProductViewModel>>(response);
        }
    }
}
