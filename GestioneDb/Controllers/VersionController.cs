using GestioneDb.Interfaces;
using GestioneDb.Services.Common;
using GestioneDb.DTOs.Versions;
using Microsoft.AspNetCore.Mvc;

namespace GestioneDb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VersionController : ControllerBase
    {
        private readonly IVersionService _versionService;

        public VersionController(IVersionService versionService)
        {
            _versionService = versionService;
        }

        /// <summary>
        /// Returns a boolean value indicating if the app version is the latest
        /// </summary>
        /// <returns>A <see cref="Result{T}"/> containing a boolean value </returns>
        [HttpGet("check-version")]
        public async Task<Result<bool>> CheckVersion()
        {
            var response = await _versionService.CheckVersionAsync(HttpContext);
            return response;
        }

        /// <summary>
        /// Returns latest app version if it is found
        /// </summary>
        /// <returns>A <see cref="Result{T}"/> containing an <see cref="UpdatePackageDTO"/> object </returns>
        [HttpGet("update-package")]
        public async Task<Result<UpdatePackageDTO>> GetUpdatePackage()
        {
            var response = await _versionService.GetUpdatePackageService();
            return response;
        }
    }
}
