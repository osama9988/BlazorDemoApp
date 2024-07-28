using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using BlazorDemoApp.Shared;
using BlazorDemoApp.Shared.Classes.BaseClass;
using BlazorDemoApp.Shared.Classes.DTO;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.IdentityModel.Tokens;

public interface ILoginServiceBackendApiHttpClient
{
    Task<ApiResponse<object>> RegisterUserAsync(Base0_MyAppUser_Register model, CancellationToken? cancellationToken = null);
    Task<ApiResponse<AuthModel>> LoginUserAsync(Base0_MyAppUser_Login model, CancellationToken? cancellationToken = null);

    Task<ApiResponse<AuthModel>> RefreshTokenAsync(string refreshToken,CancellationToken? cancellationToken = null);
}

public class LoginServiceBackendApiHttpClient : ILoginServiceBackendApiHttpClient
{
    private readonly HttpClient _httpClient;

    public LoginServiceBackendApiHttpClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<ApiResponse<object>> RegisterUserAsync(Base0_MyAppUser_Register model, CancellationToken? cancellationToken = null)
    {
        throw new NotImplementedException();
    }
    public async Task<ApiResponse<AuthModel>> LoginUserAsync(Base0_MyAppUser_Login model, CancellationToken? cancellationToken = null)
    {
        return await ApiResponse<AuthModel>.HandleExceptionAsync(async () =>
        {
            var response = await _httpClient.PostAsJsonAsync("api/auth/login", model);

            if (response == null)
            {
                throw new Exception("Failed to retrieve data from API.");
            }

            var apiResponse = new ApiResponse<AuthModel>
            {
                Success = true
            };

            if (response.IsSuccessStatusCode)
            {

                var content = await response.Content.ReadAsStringAsync();
                var apiResponse0 = JsonSerializer.Deserialize<ApiResponse<AuthModel>>(content);
                //
                var apiResponseObject = await response.Content.ReadFromJsonAsync<ApiResponse<AuthModel>>();

                apiResponse.Success = apiResponseObject.Success;
                apiResponse.Message = apiResponseObject.Message;

                if (apiResponseObject.Data != null)
                {
                    var dataJson = JsonSerializer.Serialize(apiResponseObject.Data);

                    // Log the JSON string
                    Debug.WriteLine($"Data JSON: {dataJson}");

                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    apiResponse.Data = JsonSerializer.Deserialize<AuthModel>(dataJson, options);

                    // Log the deserialized object
                    Debug.WriteLine($"Deserialized Data: {JsonSerializer.Serialize(apiResponse.Data)}");
                }
            }
            else
            {
                apiResponse.Success = false;
                apiResponse.Message = await response.Content.ReadAsStringAsync();
            }
            return apiResponse;

            //return await response.Content.ReadFromJsonAsync<ApiResponse<AuthResponse>>(cancellationToken ?? CancellationToken.None);
        });
    }
    public async Task<ApiResponse<AuthResponse>> RefreshTokenAsync(string refreshToken, CancellationToken? cancellationToken = null)
    {
        throw new NotImplementedException();
    }

    Task<ApiResponse<AuthModel>> ILoginServiceBackendApiHttpClient.RefreshTokenAsync(string refreshToken, CancellationToken? cancellationToken)
    {
        throw new NotImplementedException();
    }
}


public static class JwtTokenHelper
{

    public static List<Claim> ValidateDecodeToken(string token, IConfiguration configuration)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        //try
        //{
        //    tokenHandler.ValidateToken(token, new TokenValidationParameters
        //    {
        //        ValidateIssuerSigningKey = true,
        //        ValidateIssuer = true,
        //        ValidateAudience = false,
        //        ValidateLifetime = true,
        //        RequireExpirationTime = true,
        //        //ValidIssuer = configuration.GetValue<string>("JWTSettings:ValidIssuer"),
        //       // IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetValue<string>("JWTSettings:Secret")))
        //    }, out var validatedToken);
        //}
        //catch
        //{
        //    return new List<Claim>();
        //}

        var securityToken = tokenHandler.ReadToken(token) as JwtSecurityToken;
        return securityToken?.Claims.ToList();
    }
}

namespace BlazorApp2
{




    public class LoginService
    {
        private const string AccessToken = nameof(AccessToken);
        private const string RefreshToken = nameof(RefreshToken);

        private readonly ProtectedLocalStorage _localStorage;
        private readonly NavigationManager _navigation;
        private readonly IConfiguration _configuration;
        private readonly ILoginServiceBackendApiHttpClient _backendApiHttpClient;

        public LoginService(ProtectedLocalStorage localStorage, NavigationManager navigation, IConfiguration configuration, ILoginServiceBackendApiHttpClient backendApiHttpClient)
        {
            _localStorage = localStorage;
            _navigation = navigation;
            _configuration = configuration;
            _backendApiHttpClient = backendApiHttpClient;
        }

        public async Task<bool> LoginAsync(Base0_MyAppUser_Login model)
        {
            var response = await _backendApiHttpClient.LoginUserAsync(model);
            if (!response.Success)
                return false;

            await _localStorage.SetAsync(AccessToken, response.Data.Token);
            await _localStorage.SetAsync(RefreshToken, response.Data.Token);
           
            return true;
        }


        public async Task<List<Claim>> GetLoginInfoAsync()
        {
            var emptyResult = new List<Claim>();
            ProtectedBrowserStorageResult<string> accessToken;
            ProtectedBrowserStorageResult<string> refreshToken;
            try
            {
                accessToken = await _localStorage.GetAsync<string>(AccessToken);
                refreshToken = await _localStorage.GetAsync<string>(RefreshToken);
            }
            catch (CryptographicException)
            {
                await LogoutAsync();
                return emptyResult;
            }

            if (accessToken.Success is false || accessToken.Value == default)
                return emptyResult;

            var claims = JwtTokenHelper.ValidateDecodeToken(accessToken.Value, _configuration);


            if (claims.Count != 0)
                return claims;

            //if (refreshToken.Value != default)
            //{
            //    var response = await _backendApiHttpClient.RefreshTokenAsync(refreshToken.Value);
            //    if (string.IsNullOrWhiteSpace(response?.Result?.JwtToken) is false)
            //    {
            //        await _localStorage.SetAsync(AccessToken, response.Result.JwtToken);
            //        await _localStorage.SetAsync(RefreshToken, response.Result.RefreshToken);
            //        claims = JwtTokenHelper.ValidateDecodeToken(response.Result.JwtToken, _configuration);
            //        return claims;
            //    }
            //    else
            //    {
            //        await LogoutAsync();
            //    }
            //}
            //else
            //{
            //    await LogoutAsync();
            //}
            return claims;
        }

        public async Task LogoutAsync()
        {
            await RemoveAuthDataFromStorageAsync();
            _navigation.NavigateTo("/", true);
        }

        private async Task RemoveAuthDataFromStorageAsync()
        {
            await _localStorage.DeleteAsync(AccessToken);
            await _localStorage.DeleteAsync(RefreshToken);
        }
    }
}