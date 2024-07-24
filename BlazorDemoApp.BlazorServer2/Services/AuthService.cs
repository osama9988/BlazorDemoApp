namespace BlazorDemoApp.BlazorServer2.Services
{
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Net.Http.Json;
    using System.Threading.Tasks;
    using BlazorDemoApp.Shared.Classes.BaseClass;
    using BlazorDemoApp.Shared.Classes.DTO;
    using Blazored.SessionStorage;
    using Microsoft.AspNetCore.Identity.Data;
    using Microsoft.JSInterop;

    public class AuthService
    {
        private readonly HttpClient _httpClient;
        private readonly IJSRuntime _jsRuntime;
        private const string TokenKey = "authToken";
        private ILogger _logger;
        private CustomAuthenticationStateProvider _customAuthenticationStateProvider;
        private ApiService _apiService;
        private readonly ISessionStorageService _sessionStorageService;

        public AuthService(HttpClient httpClient, IJSRuntime jsRuntime, ILogger<AuthService> logger , CustomAuthenticationStateProvider customAuthenticationStateProvider,
            ApiService apiService, ISessionStorageService sessionStorageService)
        {
            _httpClient = httpClient;
            _jsRuntime = jsRuntime;
            _logger = logger;
            _customAuthenticationStateProvider= customAuthenticationStateProvider;
            _apiService = apiService;
            _sessionStorageService = sessionStorageService;
        }

        public async Task<bool> LoginAsync(Base0_MyAppUser_Login loginRequest)
        {
            try
            {
                var response = await _apiService.PostDataAsync<AuthModel, object>("api/auth/login", loginRequest) ;
                if (response.Success)
                {
                    await _jsRuntime.InvokeVoidAsync("localStorage.setItem", TokenKey, response.Data.Token);
                    await _sessionStorageService.SetItemAsync("token", response.Data.Token);
                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", response.Data.Token);
                    _customAuthenticationStateProvider.MarkUserAsAuthenticated(response.Data.Token);
                    return true;
                }
                return false;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return false;
            }
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
