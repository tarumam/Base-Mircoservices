using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using BaseProject.WebAPI.Core.User;

namespace BaseProject.WebAPI.Core.DelegatingHandlers
{
    public class HttpClientAuthBlueSoftDelegatingHandler : DelegatingHandler
    {
        private readonly IAspNetUser _user;

        public HttpClientAuthBlueSoftDelegatingHandler(IAspNetUser user)
        {
            _user = user;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var bluesoftToken = _user.GetHttpContext().Request.Headers["X-Cosmos-Token"];

            if (!string.IsNullOrEmpty(bluesoftToken))
            {
                request.Headers.Add("X-Cosmos-Token", "teMPAh28jA0d3aO149-c-w");
            }
            return base.SendAsync(request, cancellationToken);
        }
    }
}
