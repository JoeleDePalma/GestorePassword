using System;
using System.Collections.Generic;
using System.Text;
using Libreria.API;
using Libreria.HTTPRequestsLibrary;
using GestorePassword.Core.Models;

public static class AppServices
{
    public static ApiClient apiClient { get; set; }
    public static UserApi userApi { get; set; }
    public static PasswordApi passwordApi { get; set; }
    public static VersionApi versionApi { get; set; }
    public static List<PasswordInfo> passwordList { get; set; }
    public static UserInfo currentUser { get; set; }
    public static Version appVersion { get; set; }
}

