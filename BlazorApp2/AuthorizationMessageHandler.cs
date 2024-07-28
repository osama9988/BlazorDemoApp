namespace BlazorApp2
{
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

    public class AuthorizationMessageHandler : DelegatingHandler
    {
        private const string AccessToken = nameof(AccessToken);
        private const string RefreshToken = nameof(RefreshToken);
        private readonly ProtectedLocalStorage _localStorage;

        public AuthorizationMessageHandler(ProtectedLocalStorage localStorage)
        {
            _localStorage = localStorage;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (request.Properties.ContainsKey("UserHttpContext"))
            {
                var token = await _localStorage.GetAsync<string>(AccessToken);
                if (!string.IsNullOrEmpty(token.Value))
                {
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token.Value);
                }
            }
            return await base.SendAsync(request, cancellationToken);
        }
    }
}
