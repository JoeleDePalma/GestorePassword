using GestioneDb.DTOs.Versions;
using GestioneDb.Services.Common;

namespace GestioneDb.Interfaces
{
    public interface IVersionService
    {
        /// <summary>
        /// Checks if the app version is in the headers and if the middleware was not compromised during the request
        /// </summary>
        /// <param name="context">The HTTP context where the app version is searched </param>
        /// <returns>A boolean value indicating if the app version is the latest </returns>
        Task<Result<bool>> CheckVersionAsync(HttpContext context);

        /// <summary>
        /// Searches for the latest app version zip file
        /// </summary>
        /// <returns>
        /// A <see cref="Result{T}"/> containing an <see cref="UpdatePackageDTO"/> object 
        /// if the zip exists, otherwise an error
        /// </returns>
        Task<Result<UpdatePackageDTO>> GetUpdatePackageService();
    }
}
