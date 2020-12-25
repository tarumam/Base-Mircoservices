using System;
using System.Net.Http;
using BaseProjetct.Bff.Shopping.Extensions;
using Microsoft.Extensions.Options;

namespace BaseProjetct.Bff.Shopping.Service
{
    public interface IPaymentService { }
    public class PaymentService : Service, IPaymentService
    {
        private readonly HttpClient _httpClient;

        public PaymentService(HttpClient httpClient, IOptions<AppServicesSettings> settings)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(settings.Value.ShoppingCartUrl);
        }
    }
}
