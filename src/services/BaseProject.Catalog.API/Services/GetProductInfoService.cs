using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using BaseProject.Catalog.API.Application.Commands;
using BaseProject.Catalog.API.Models;
using BaseProject.Catalog.Domain;
using BaseProject.Core.Mediatr;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BaseProject.Catalog.API.Services
{
    public class GetProductInfoService : BackgroundService
    {
        private readonly ILogger _logger;
        private readonly IServiceProvider _serviceProvider;

        private Timer _timer;
        private HttpClient _httpClient;

        public GetProductInfoService(ILogger<GetProductInfoService> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("GetProductInfo - Iniciando ciclo em 3 minutos.");

            _timer = new Timer(BuscaInformacoes, null, TimeSpan.Zero, TimeSpan.FromSeconds(320));

            return Task.CompletedTask;
        }

        private async void BuscaInformacoes(object state)
        {
            _logger.LogInformation("GetProductInfo - Iniciando Busca");

            List<Product> products;

            using (var scope = _serviceProvider.CreateScope())
            {
                var productRep = scope.ServiceProvider.GetRequiredService<IProductRepository>();

                if (!productRep.UnitOfWork.IsDBHelthy())
                {
                    return;
                }

                products = await productRep.GetProductsWithPendingBaseInfo();
            }

            if (products != null && !products.Any())
            {
                _logger.LogInformation($"GetProductInfo - Nenhum produto encontrado");
                return;
            }

            _logger.LogInformation($"GetProductInfo - Quantidade de produtos pendentes: {products.Count}");

            var product = products.FirstOrDefault();
            if (string.IsNullOrEmpty(product.Barcode))
            {
                _logger.LogInformation($"GetProductInfo - Código de barras vazio - id: {product.Id}");
                return;
            }

            _logger.LogInformation($"GetProductInfo - Buscando informações do código: {product.Barcode}");


            if (_httpClient == null)
                _httpClient = new HttpClient();

            if (!_httpClient.DefaultRequestHeaders.Contains("X-Cosmos-Token"))
            {
                _httpClient.DefaultRequestHeaders.Add("X-Cosmos-Token", "teMPAh28jA0d3aO149-c-w");
            }
            var response = await _httpClient.GetAsync($"https://api.cosmos.bluesoft.com.br/gtins/{product.Barcode}");

            _logger.LogInformation($"GetProductInfo - Sucesso na request: {response.IsSuccessStatusCode}");

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            BlueSoftDTO result;

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var data = await response.Content.ReadAsStringAsync();
                result = JsonSerializer.Deserialize<BlueSoftDTO>(data, options);
            }
            else
            {
                _logger.LogInformation($"GetProductInfo - fatlha na request do bluesoft");

                result = new BlueSoftDTO();
                result.Description = "Produto não encontrado.";
                result.Thumbnail = null;
                result.Gross_weight = null;
            }

            using (var scope = _serviceProvider.CreateScope())
            {
                var mediator = scope.ServiceProvider.GetRequiredService<IMediatorHandler>();
                var success = await mediator.SendCommand(new UpdateProductCommand(product.Id, product.Barcode, result.Description, $"peso: {result.Gross_weight}", true, result.Thumbnail, true));
            }

            _logger.LogInformation($"GetProductInfo - Fim de um ciclo");
        }


    }
}
