using Libreria.HTTPRequestsLibrary.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Libreria.HTTPRequestsLibrary.Interfaces
{
    public interface IVersionApi
    {
        /// <summary>
        /// Send a GET request to the server to check if the current app version is valid
        /// </summary>
        /// <returns>
        /// An <see cref="ApiResponse{T}"/> containg a boolean value
        /// </returns>
        Task<ApiResponse<bool>> CheckVersionAsync();
    }
}
