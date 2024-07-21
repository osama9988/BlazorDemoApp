using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;


namespace BlazorDemoApp.BlazorServer.Services
{
  
    public class AuthenticatedHttpClientHandler : DelegatingHandler
    {
        private readonly IJSRuntime _jsRuntime;
        private readonly NavigationManager _navigationManager;
        private readonly CustomAuthenticationStateProvider _authenticationStateProvider;

        public AuthenticatedHttpClientHandler(
            IJSRuntime jsRuntime,
            NavigationManager navigationManager,
            CustomAuthenticationStateProvider authenticationStateProvider)
        {
            _jsRuntime = jsRuntime;
            _navigationManager = navigationManager;
            _authenticationStateProvider = authenticationStateProvider;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var token = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "authToken");
            if (!string.IsNullOrEmpty(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var response = await base.SendAsync(request, cancellationToken);

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                await _authenticationStateProvider.MarkUserAsLoggedOut();
                var returnUrl = _navigationManager.Uri;
                _navigationManager.NavigateTo($"login?returnUrl={Uri.EscapeDataString(returnUrl)}");
            }

            return response;
        }
    }

}
