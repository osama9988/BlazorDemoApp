using BlazorDemoApp.Shared;

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
    }

}
