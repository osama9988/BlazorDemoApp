namespace BlazorDemoApp.BlazorServer2.Services
{
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Net.Http.Json;
    using System.Threading.Tasks;
    using BlazorDemoApp.Shared.Classes.BaseClass;
    using BlazorDemoApp.Shared.Classes.DTO;
    using Microsoft.AspNetCore.Identity.Data;
    using Microsoft.JSInterop;

    public class AuthService
    {
        private readonly HttpClient _httpClient;
        private readonly IJSRuntime _jsRuntime;
        private const string TokenKey = "authToken";

        public AuthService(HttpClient httpClient, IJSRuntime jsRuntime)
        {
            _httpClient = httpClient;
            _jsRuntime = jsRuntime;
        }

        public async Task<bool> LoginAsync(Base0_MyAppUser_Login loginRequest)
        {
            var response = await _httpClient.PostAsJsonAsync("api/auth/login", loginRequest);
            if (response.IsSuccessStatusCode)
            {
                var authResponse = await response.Content.ReadFromJsonAsync<AuthResponse>();
                await _jsRuntime.InvokeVoidAsync("localStorage.setItem", TokenKey, authResponse.Token);
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authResponse.Token);
                return true;
            }
            return false;
        }

        public async Task LogoutAsync()
        {
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", TokenKey);
            _httpClient.DefaultRequestHeaders.Authorization = null;
        }

        public async Task InitializeAsync()
        {
            var token = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", TokenKey);
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
        }
    }

}
