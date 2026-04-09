using Microsoft.IO;
using System.Text.Json;
using GestioneDb.Services.Common;

namespace GestioneDb.Middlewares
{
    public class CatchStatusCodeMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly RecyclableMemoryStreamManager _memoryManager = new();

        public CatchStatusCodeMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        /// <summary>
        /// Intercepts the HTTP response, extracts the custom <see cref="StatusCode"/> returned by the application,
        /// maps it to a real HTTP status code, and rewrites the response in a standardized API format
        /// </summary>
        /// <param name="context">The HTTP context of the current request </param>
        public async Task InvokeAsync(HttpContext context)
        {
            var originalBody = context.Response.Body;
            var tempStream = _memoryManager.GetStream();
            context.Response.Body = tempStream;

            try
            {
                await _next(context);

                tempStream.Seek(0, SeekOrigin.Begin);
                var bodyText = await new StreamReader(tempStream).ReadToEndAsync();

                bool isJsonResponse = context.Response.ContentType?.Contains("application/json") ?? false;

                if (!isJsonResponse)
                {
                    tempStream.Seek(0, SeekOrigin.Begin);
                    await tempStream.CopyToAsync(originalBody);
                    return;
                }

                var result = JsonSerializer.Deserialize<Result<object>>(bodyText, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (result == null)
                {
                    tempStream.Seek(0, SeekOrigin.Begin);
                    await tempStream.CopyToAsync(originalBody);
                    return;
                }

                int httpStatus = MapStatusCode(result.responseStatusCode);

                if (httpStatus == StatusCodes.Status204NoContent)
                {
                    context.Response.Body = originalBody;
                    context.Response.StatusCode = StatusCodes.Status204NoContent;
                    return;
                }

                var apiResponse = new ApiResponse<object>
                {
                    Success = result.responseStatusCode == StatusCode.Ok ||
                              result.responseStatusCode == StatusCode.Created ||
                              result.responseStatusCode == StatusCode.NoContent,
                    Message = result.ErrorString,
                    Data = result.Data
                };

                string json = JsonSerializer.Serialize(apiResponse);

                context.Response.Body = originalBody;
                context.Response.StatusCode = httpStatus;
                context.Response.ContentType = "application/json";

                await context.Response.WriteAsync(json);
            }
            finally
            {
                tempStream.Dispose();
            }
        }

        private int MapStatusCode(StatusCode code)
        {
            return code switch
            {
                StatusCode.Ok => StatusCodes.Status200OK,
                StatusCode.Created => StatusCodes.Status201Created,
                StatusCode.NoContent => StatusCodes.Status204NoContent,
                StatusCode.BadRequest => StatusCodes.Status400BadRequest,
                StatusCode.Unauthorized => StatusCodes.Status401Unauthorized,
                StatusCode.NotFound => StatusCodes.Status404NotFound,
                StatusCode.Conflict => StatusCodes.Status409Conflict,
                StatusCode.InternalServerError => StatusCodes.Status500InternalServerError,
                StatusCode.UpgradeRequired => StatusCodes.Status426UpgradeRequired,
                _ => StatusCodes.Status500InternalServerError
            };
        }
    }
}