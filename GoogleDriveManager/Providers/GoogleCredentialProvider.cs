using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Sheets.v4;
using Google.Apis.Util.Store;

namespace GoogleDriveManager.Providers
{
    public static class GoogleCredentialProvider
    {
        private static UserCredential? _credential;

        public static async Task<UserCredential> GetCredentialAsync()
        {
            if (_credential != null)
                return _credential;

            using (var stream = new FileStream(Constants.Constants.CredentialsFilePath, FileMode.Open, FileAccess.Read))
            {
                _credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.FromStream(stream).Secrets,
                    new[] { DriveService.Scope.Drive, SheetsService.Scope.Spreadsheets },
                    "user",
                    CancellationToken.None,
                    new FileDataStore("TokenStore"));
            }

            return _credential;
        }
    }
}