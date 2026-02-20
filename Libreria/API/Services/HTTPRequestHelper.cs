using System.Net.Http.Json;

namespace HTTPRequestsLibrary.Services
{
    public class HTTPRequestHelper
    {
        public static async Task<ApiResponse<T>> SendAsync<T>(Func<Task<HttpResponseMessage>> httpCall)
        {
            var response = await httpCall();

            var apiResponse = new ApiResponse<T>
            {
                Success = response.IsSuccessStatusCode,
                StatusCode = (int) response.StatusCode
            };

            if (!apiResponse.Success)
            {
                apiResponse.Error = await response.Content.ReadAsStringAsync();
                apiResponse.Data = default;
                return apiResponse;
            }

            apiResponse.Data = await response.Content.ReadFromJsonAsync<T>();
            return apiResponse;
        }
    }
}
