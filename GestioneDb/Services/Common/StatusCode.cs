namespace GestioneDb.Services.Common
{
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
        InternalServerError
    }
}
