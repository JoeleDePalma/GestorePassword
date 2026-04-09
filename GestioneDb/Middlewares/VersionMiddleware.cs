using System;

namespace GestioneDb.Middlewares
{
    /// <summary>
    /// Reads the client app version from the HTTP request header "GestorePassword-version"
    /// and stores it inside <c>HttpContext.Items</c> so controllers can check it later.
    ///
    /// If the header is missing or invalid, nothing is stored.
    /// The request continues normally
    /// </summary>
    public class VersionMiddleware
    {
        private readonly RequestDelegate _next;

        /// <summary>
        /// Constructor. Receives the next middleware in the pipeline
        /// </summary>
        public VersionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        /// <summary>
        /// Runs for every HTTP request.
        /// Tries to read the "GestorePassword-version" header.
        /// If the header contains a valid version number,
        /// it is saved in <c>HttpContext.Items</c> with the key "GestorePassword-version"
        /// </summary>
        /// <param name="context">The HTTP context of the current request </param>
        public async Task Invoke(HttpContext context)
        {
            // Check if the request contains the version header
            if (context.Request.Headers.TryGetValue("GestorePassword-version", out var header))
            {
                // Try to convert the header value into a Version object
                if (Version.TryParse(header!, out var version))
                {
                    // Save the version in the HttpContext for later use
                    context.Items["GestorePassword-version"] = version;
                }
            }

            // Continue to the next middleware or controller
            await _next(context);
        }
    }
}