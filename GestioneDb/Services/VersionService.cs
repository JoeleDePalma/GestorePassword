using GestioneDb.DTOs.Versions;
using GestioneDb.Interfaces;
using GestioneDb.Services.Common;

namespace GestioneDb.Services
{
    public class VersionService : IVersionService
    {
        private readonly ControllersServices.ControllersServices _services;
        public VersionService(ControllersServices.ControllersServices services)
        {
            _services = services;
        }

        /// <summary>
        /// Checks if the app version is in the headers and if the middleware was not compromised during the request
        /// </summary>
        /// <param name="context">The HTTP context where the app version is searched </param>
        /// <returns>A boolean value indicating if the app version is the latest </returns>
        public async Task<Result<bool>> CheckVersionAsync(HttpContext context)
        {
            if (!context.Items.TryGetValue("GestorePassword-version", out var versionObj))
                return Result<bool>.Fail(StatusCode.BadRequest, "Aggiornamento disponibile!");

            if (versionObj is not Version version)
                return Result<bool>.Fail(StatusCode.BadRequest, "Aggiornamento disponibile!"); ;

            bool check = version >= new Version("1.2.0");

            if (!check)
            {
                return Result<bool>.Fail(StatusCode.UpgradeRequired, "Aggiornamento disponibile!");
            }

            return Result<bool>.Ok(true, StatusCode.Ok);
        }

        /// <summary>
        /// Searches for the latest app version zip file
        /// </summary>
        /// <returns>
        /// A <see cref="Result{T}"/> containing an <see cref="UpdatePackageDTO"/> object 
        /// if the zip exists, otherwise an error
        /// </returns>
        public async Task<Result<UpdatePackageDTO>> GetUpdatePackageService()
        {
            try
            {
                var folder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Sources", "LatestAppVersion");

                var zip = Directory.GetFiles(folder, "*.zip").FirstOrDefault();

                if (zip is null)
                    return Result<UpdatePackageDTO>.Fail(StatusCode.NotFound, "Nessun pacchetto di aggiornamento trovato.");

                var bytes = await File.ReadAllBytesAsync(zip);

                var dto = new UpdatePackageDTO
                {
                    FileName = Path.GetFileName(zip),
                    Content = bytes
                };

                return Result<UpdatePackageDTO>.Ok(dto, StatusCode.Ok);
            }
            catch (Exception ex)
            {
                return Result<UpdatePackageDTO>.Fail(StatusCode.InternalServerError, "Errore durante il recupero del pacchetto di aggiornamento.");
            }
        }
    }
}