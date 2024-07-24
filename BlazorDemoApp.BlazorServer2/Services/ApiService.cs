using BlazorDemoApp.Shared;
using BlazorDemoApp.Shared.Classes.DTO;
using Microsoft.AspNetCore.Identity.Data;
using System.Diagnostics;
using System.Text.Json;

namespace BlazorDemoApp.BlazorServer2.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;

        public ApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ApiResponse<T>> GetDataAsync<T>(string url)
        {
            try
            {
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
                    var apiResponse0 = JsonSerializer.Deserialize<ApiResponse<AuthModel>>(content);
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
