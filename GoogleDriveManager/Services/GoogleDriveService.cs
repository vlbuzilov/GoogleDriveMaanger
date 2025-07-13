using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using GoogleDriveManager.Utils;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GoogleDriveManager.Services
{
    public class GoogleDriveService
    {
        private readonly DriveService _driveService;

        public GoogleDriveService()
        {
            var clientId = AppSettingsHelper.GetAppSetting("GoogleApi", "ClientId");
            var clientSecret = AppSettingsHelper.GetAppSetting("GoogleApi", "ClientSecret");
            var applicationName = AppSettingsHelper.GetAppSetting("GoogleApi", "ApplicationName");
            var username = AppSettingsHelper.GetAppSetting("GoogleApi", "Username");
            var accessToken = AppSettingsHelper.GetAppSetting("GoogleApi", "AccessToken");
            var refreshToken = AppSettingsHelper.GetAppSetting("GoogleApi", "RefreshToken");

            var tokenResponse = new TokenResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };

            var apiCodeFlow = new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer
            {
                ClientSecrets = new ClientSecrets
                {
                    ClientId = clientId,
                    ClientSecret = clientSecret
                },
                Scopes = new[] { DriveService.Scope.Drive },
                DataStore = new FileDataStore(applicationName)
            });

            var credential = new UserCredential(apiCodeFlow, username, tokenResponse);

            _driveService = new DriveService(new BaseClientService.Initializer
            {
                HttpClientInitializer = credential,
                ApplicationName = applicationName
            });
        }

        public async Task<IList<Google.Apis.Drive.v3.Data.File>> GetFilesAsync()
        {
            var request = _driveService.Files.List();
            request.Fields = "files(id, name, mimeType, modifiedTime)";
            var result = await request.ExecuteAsync();
            return result.Files;
        }
    }
}
