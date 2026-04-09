using Installer.HTTPRequestsLibrary;
using Installer.HTTPRequestsLibrary.Services;
using Installer.DTOs.Versions;

namespace Installer.API
{
    public class VersionApi
    {
        private readonly ApiClient _client;

        public VersionApi(ApiClient client)
        {
            _client = client;
        }

        /// <summary>
        /// Sends a request to the server to get the latest version app zip
        /// </summary>
        /// <returns>An <see cref="ApiResponse{T}"/> containing an <see cref="UpdatePackageDTO"/> object </returns>
        public async Task<ApiResponse<UpdatePackageDTO>> GetLatestAppVersionZipAsync()
            => await HTTPRequestHelper.SendAsync<UpdatePackageDTO>(
                () => _client.Http.GetAsync("api/version/update-package")
                );
    }
}
