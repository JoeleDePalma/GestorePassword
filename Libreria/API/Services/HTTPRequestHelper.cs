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

            var apiResponse = new ApiResponse<T>();

            if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                apiResponse.Success = true;
                apiResponse.Message = "No content";
                apiResponse.Data = default;
                return apiResponse;
            }

            if (string.IsNullOrWhiteSpace(raw))
            {
                apiResponse.Success = response.IsSuccessStatusCode;
                apiResponse.Message = response.IsSuccessStatusCode
                    ? "No content"
                    : "Empty response body";
                apiResponse.Data = default;
                return apiResponse;
            }

            try
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var wrapper = JsonSerializer.Deserialize<ApiResponse<T>>(raw, options);

                if (wrapper != null)
                {
                    return wrapper;
                }

                var dto = JsonSerializer.Deserialize<T>(raw, options);

                apiResponse.Success = true;
                apiResponse.Data = dto;
                apiResponse.Message = null;

                return apiResponse;
            }
            catch (Exception ex)
            {
                return new ApiResponse<T>
                {
                    Success = false,
                    Message = $"Deserialization error: {ex.Message}",
                    Data = default
                };
            }
        }
    }
}
