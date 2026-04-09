namespace GestioneDb.Services.Common.Exceptions
{
    /// <summary>
    /// Exception used when something goes wrong during the password retrieval of an user
    /// </summary>
    public class PasswordRetrievalException : Exception
    {
        public PasswordRetrievalException(string? message) : base(message)
        {
        }
    }
}
