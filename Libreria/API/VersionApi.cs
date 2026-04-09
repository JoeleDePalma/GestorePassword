using Libreria.HTTPRequestsLibrary;
using Libreria.HTTPRequestsLibrary.Services;
using Libreria.DTOs.Versions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Libreria.API
{
    public class VersionApi
    {
        private readonly ApiClient _client;

        public VersionApi(ApiClient client)
        {
            _client = client;
        }

        /// <summary>
        /// Send a GET request to the server to check if the current app version is valid
        /// </summary>
        /// <returns>
        /// An <see cref="ApiResponse{T}"/> containg a boolean value
        /// </returns>
        public async Task<ApiResponse<bool>> CheckVersionAsync()
            => await HTTPRequestHelper.SendAsync<bool>(
                () => _client.Http.GetAsync("api/version/check-version")
                );
    }
}
