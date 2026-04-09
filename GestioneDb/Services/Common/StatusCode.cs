namespace GestioneDb.Services.Common
{
    /// <summary>
    /// Represents the set of HTTP status codes used throughout the application
    /// to standardize controller responses.
    /// </summary>
    public enum StatusCode
    {
        Ok,
        Created,
        NoContent,

        NotFound,
        Unauthorized,
        Conflict,
        InvalidInput,
        Forbidden,
        BadRequest,

        InternalServerError,

        UpgradeRequired
    }
}