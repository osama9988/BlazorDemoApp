
using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BlazorDemoApp.BlazorServer2.Services
{
    

    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly IJSRuntime _jsRuntime;
        private readonly HttpClient _httpClient;
        private readonly ISessionStorageService _sessionStorageService;

        private const string TokenKey = "authToken";
        private string _username;
        private string _userId;

        public CustomAuthenticationStateProvider(IJSRuntime jsRuntime, HttpClient httpClient, ISessionStorageService sessionStorageService)
        {
            _jsRuntime = jsRuntime;
            _httpClient = httpClient;
            _sessionStorageService = sessionStorageService;
        }

        private ClaimsPrincipal ParseToken(string token)
        {
            var claims = JwtParser.ParseClaimsFromJwt(token);
            var identity = new ClaimsIdentity(claims, "jwt");

            _username = JwtParser.GetClaimValue(claims, ClaimTypes.Name.ToString());
            _userId = JwtParser.GetClaimValue(claims, ClaimTypes.NameIdentifier.ToString());
            return new ClaimsPrincipal(identity);
        }
        public async Task MarkUserAsAuthenticated(string token)
        {
            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", TokenKey, token);

            var user = ParseToken(token);
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
        }

        public async Task MarkUserAsLoggedOut()
        {
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", TokenKey);
            _httpClient.DefaultRequestHeaders.Authorization = null;
            var identity = new ClaimsIdentity();
            var user = new ClaimsPrincipal(identity);
            _username = null;
            _userId = null;
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {

                var token = await _sessionStorageService.GetItemAsync<string>("token");
                if (string.IsNullOrEmpty(token))
                {
                    return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
                }
                var identity = new ClaimsIdentity(JwtParser.ParseClaimsFromJwt(token), "jwt");
                var user = new ClaimsPrincipal(identity);
                return await Task.FromResult(new AuthenticationState(user));

                //// Use asynchronous interop call
                //var token = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "authToken");

                //if (string.IsNullOrEmpty(token))
                //{
                //    // No token found
                //    return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
                //}
            }
            catch (Exception ex)
            {
                // Handle exceptions
                Console.WriteLine($"Error in GetAuthenticationStateAsync: {ex.Message}");
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }


        }

        public string GetUsername() => _username;
        public string GetUserId() => _userId;
    }

    public static class JwtParser
    {
        public static IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
        {
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(jwt);
            return token.Claims;
        }

        public static string GetClaimValue(IEnumerable<Claim> claims, string claimType)
        {
            return claims.FirstOrDefault(c => c.Type == claimType)?.Value;
        }
    }

    //public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    //{
    //    private readonly IJSRuntime _jsRuntime;
    //    private readonly HttpClient _httpClient;
    //    private const string TokenKey = "authToken";

    //    public CustomAuthenticationStateProvider(IJSRuntime jsRuntime, HttpClient httpClient)
    //    {
    //        _jsRuntime = jsRuntime;
    //        _httpClient = httpClient;
    //    }

    //    public async Task MarkUserAsAuthenticated(string token)
    //    {
    //        await _jsRuntime.InvokeVoidAsync("localStorage.setItem", TokenKey, token);
    //        var identity = new ClaimsIdentity(JwtParser.ParseClaimsFromJwt(token), "jwt");
    //        var user = new ClaimsPrincipal(identity);
    //        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
    //    }

    //    public async Task MarkUserAsLoggedOut()
    //    {
    //        await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", TokenKey);
    //        var identity = new ClaimsIdentity();
    //        var user = new ClaimsPrincipal(identity);
    //        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
    //    }

    //    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    //    {
    //        var token = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", TokenKey);
    //        if (string.IsNullOrEmpty(token))
    //        {
    //            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
    //        }

    //        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    //        var identity = new ClaimsIdentity(JwtParser.ParseClaimsFromJwt(token), "jwt");
    //        var user = new ClaimsPrincipal(identity);

    //        return new AuthenticationState(user);
    //    }
    //}

}