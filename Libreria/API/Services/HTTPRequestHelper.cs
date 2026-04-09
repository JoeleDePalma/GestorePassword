using System.Text.Json;

namespace Libreria.HTTPRequestsLibrary.Services
{
    /// <summary>
    /// Helper class used to send HTTP requests and read the response
    /// as an <c>ApiResponse{T}</c> object
    /// </summary>
    public class HTTPRequestHelper
    {
        /// <summary>
        /// Sends an HTTP request and tries to read the server response
        /// as an <c>ApiResponse{T}</c>. If the server does not return
        /// this format, the method tries to read the raw JSON as <typeparamref name="T"/>
        /// </summary>
        /// <typeparam name="T">The type of data expected from the server </typeparam>
        /// <param name="httpCall">
        /// A function that performs the HTTP request and returns an <c>HttpResponseMessage</c>
        /// </param>
        /// <returns>
        /// An <c>ApiResponse{T}</c> object containing the result of the request
        /// </returns>
        public static async Task<ApiResponse<T>> SendAsync<T>(Func<Task<HttpResponseMessage>> httpCall)
        {
            // Execute the HTTP request
            var response = await httpCall();

            // Read the raw response body as a string
            var raw = await response.Content.ReadAsStringAsync();

            // Create a default ApiResponse object
            var apiResponse = new ApiResponse<T>();

            // Case 1: The server returned HTTP 204 No Content
            if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                apiResponse.Success = true;
                apiResponse.Message = "No content";
                apiResponse.Data = default!;
                return apiResponse;
            }

            // Case 2: The response body is empty or whitespace
            if (string.IsNullOrWhiteSpace(raw))
            {
                apiResponse.Success = response.IsSuccessStatusCode;
                apiResponse.Message = response.IsSuccessStatusCode
                    ? "No content"
                    : "Empty response body";
                apiResponse.Data = default!;
                return apiResponse;
            }

            try
            {
                // JSON options: ignore case in property names
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                // Try to read the response as ApiResponse<T>
                var wrapper = JsonSerializer.Deserialize<ApiResponse<T>>(raw, options);

                // If the server returned the expected wrapper format, return it
                if (wrapper != null)
                {
                    return wrapper;
                }

                // If not, try to read the raw JSON directly as T
                var dto = JsonSerializer.Deserialize<T>(raw, options);

                apiResponse.Success = true;
                apiResponse.Data = dto!;
                apiResponse.Message = null!;

                return apiResponse;
            }
            catch (Exception ex)
            {
                // If JSON parsing fails, return an error response
                return new ApiResponse<T>
                {
                    Success = false,
                    Message = $"Deserialization error: {ex.Message}",
                    Data = default!
                };
            }
        }
    }
}