using System.Net.Http.Json;
using System.Text.Json;

namespace HTTPRequestsLibrary.Services
{
    public class HTTPRequestHelper
    {
        public static async Task<ApiResponse<T>> SendAsync<T>(Func<Task<HttpResponseMessage>> httpCall)
        {
            var response = await httpCall();

            var raw = await response.Content.ReadAsStringAsync();

            Console.WriteLine($"RAW RESPONSE: {raw}");
            Console.WriteLine($"STATUS CODE: {response.StatusCode}");

            var apiResponse = new ApiResponse<T>
            {
                Success = response.IsSuccessStatusCode,
                StatusCode = (int) response.StatusCode
            };

            if (!apiResponse.Success)
            {
                apiResponse.ErrorString = await response.Content.ReadAsStringAsync();
                apiResponse.Data = default;
                return apiResponse;
            }

            if (string.IsNullOrWhiteSpace(raw))
            {
                apiResponse.Data = default;
                return apiResponse;
            }

            try
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    PropertyNamingPolicy = null
                };

                // Se il JSON contiene "data": è ApiResponse<T>
                if (raw.Contains("\"data\":{") || raw.Contains("\"data\":["))
                {
                    var wrapper = await response.Content.ReadFromJsonAsync<ApiResponse<T>>(options);

                    if (wrapper != null)
                    {
                        apiResponse.Success = wrapper.Success;
                        apiResponse.Error = wrapper.Error;
                        apiResponse.ErrorString = wrapper.ErrorString;
                        apiResponse.Data = wrapper.Data;
                    }
                    else
                    {
                        apiResponse.Success = false;
                        apiResponse.ErrorString = "Wrapper null";
                    }
                }
                else
                {
                    // DTO piatto (Users/Register)
                    var dto = await response.Content.ReadFromJsonAsync<T>(options);
                    apiResponse.Data = dto;
                    apiResponse.Success = true;
                }
            }
            catch (Exception ex)
            {
                apiResponse.Success = false;
                apiResponse.ErrorString = $"Deserialization error: {ex.Message}";
                apiResponse.Data = default;
            }

            return apiResponse;
        }
    }
}
