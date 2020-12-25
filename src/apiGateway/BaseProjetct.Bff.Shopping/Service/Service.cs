using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using BaseProject.Core.Communication;

namespace BaseProjetct.Bff.Shopping.Service
{
    public abstract class Service
    {
        protected StringContent GetContent(object dado)
        {
            return new StringContent(
                JsonSerializer.Serialize(dado),
                Encoding.UTF8,
                "application/json");
        }

        protected async Task<T> DeserializeObjetoResponse<T>(HttpResponseMessage responseMessage)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            return JsonSerializer.Deserialize<T>(await responseMessage.Content.ReadAsStringAsync(), options);
        }

        protected bool HandleErrorsResponse(HttpResponseMessage response)
        {
            if (response.StatusCode == HttpStatusCode.BadRequest) return false;

            response.EnsureSuccessStatusCode();

            return true;
        }

        protected ResponseResult ReturnOk()
        {
            return new ResponseResult();
        }
    }
}
