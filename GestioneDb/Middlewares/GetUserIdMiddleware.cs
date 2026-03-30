using System.Security.Claims;

namespace GestioneDb.Middlewares
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class SkipUserIdExtractionAttribute : Attribute
    {
    }

    public class GetUserIdMiddleware
    {
        private readonly RequestDelegate _next;

        public GetUserIdMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var endpoint = context.GetEndpoint();

            var skip = endpoint?.Metadata.GetMetadata<SkipUserIdExtractionAttribute>() != null;

            if (skip)
            {
                await _next(context);
                return;
            }

            if (context.User.Identity?.IsAuthenticated == true)
            {
                var userIdClaim = context.User.FindFirst(ClaimTypes.NameIdentifier);

                if (userIdClaim != null)
                {
                    context.Items["UserId"] = int.Parse(userIdClaim.Value);
                }
            }

            await _next(context);
        }
    }
}