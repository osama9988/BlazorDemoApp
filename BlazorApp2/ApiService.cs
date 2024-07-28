using BlazorDemoApp.Shared;
using BlazorDemoApp.Shared.Classes.DTO;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Identity.Data;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Text.Json;

namespace BlazorApp2
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;
        private readonly ProtectedLocalStorage _localStorage;

        public ApiService(HttpClient httpClient,  ProtectedLocalStorage localStorage)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
        }

        public async Task<ApiResponse<T>> GetDataAsync<T>(string url)
        {
            try
            {
                var tokenResult = await _localStorage.GetAsync<string>("AccessToken");
               
                if (!string.IsNullOrEmpty(tokenResult.Value))
                {
                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenResult.Value);
                }

                var response = await _httpClient.GetFromJsonAsync<ApiResponse<T>>(url);
                if (response == null)
                {
                    throw new Exception("Failed to retrieve data from API.");
                }

                return response;
            }
            catch (HttpRequestException ex)
            {
                // Handle network-related errors
                return new ApiResponse<T>
                {
                    Success = false,
                    Message = "Network error occurred.",
                    Errors = new List<ApiError> { new ApiError { Field = "", Message = ex.Message } }
                };
            }
            catch (Exception ex)
            {
                // Handle other errors
                return new ApiResponse<T>
                {
                    Success = false,
                    Message = "An error occurred.",
                    Errors = new List<ApiError> { new ApiError { Field = "", Message = ex.Message } }
                };
            }
        }

        public async Task<ApiResponse<T>> PostDataAsync<T, TT>(string url, TT model)
        {
            try
            {
                var tokenResult = await _localStorage.GetAsync<string>("AccessToken");

                if (!string.IsNullOrEmpty(tokenResult.Value))
                {
                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenResult.Value);
                }

                var response = await _httpClient.PostAsJsonAsync(url, model);
                if (response == null)
                {
                    throw new Exception("Failed to retrieve data from API.");
                }

                var apiResponse = new ApiResponse<T>
                {
                    Success = true
                };

                if (response.IsSuccessStatusCode)
                {

                    var content = await response.Content.ReadAsStringAsync();
                    var apiResponse0 = JsonSerializer.Deserialize<ApiResponse<T>>(content);
                    //
                    var apiResponseObject = await response.Content.ReadFromJsonAsync<ApiResponse<object>>();

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
                        apiResponse.Data = JsonSerializer.Deserialize<T>(dataJson, options);

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
            }
            catch (HttpRequestException ex)
            {
                // Handle network-related errors
                return new ApiResponse<T>
                {
                    Success = false,
                    Message = "Network error occurred.",
                    Errors = new List<ApiError> { new ApiError { Field = "", Message = ex.Message } }
                };
            }
            catch (Exception ex)
            {
                // Handle other errors
                return new ApiResponse<T>
                {
                    Success = false,
                    Message = "An error occurred.",
                    Errors = new List<ApiError> { new ApiError { Field = "", Message = ex.Message } }
                };
            }
        }
    }

}
