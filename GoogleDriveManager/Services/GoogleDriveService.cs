using System.Collections.Generic;
using System.Threading.Tasks;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using GoogleDriveManager.Utils;

namespace GoogleDriveManager.Services
{
    public class GoogleDriveService
    {
        private readonly DriveService _driveService;
        private readonly string _folderId;

        public GoogleDriveService(DriveService driveService)
        {
            _driveService = driveService;
            _folderId = AppSettingsHelper.GetAppSetting("folderId");
        }

        public async Task<IEnumerable<Google.Apis.Drive.v3.Data.File>> GetFilesAsync()
        {
            var request = _driveService.Files.List();
            request.Q = $"'{_folderId}' in parents";
            request.Fields = "files(id, name, mimeType, modifiedTime)";

            return (await request.ExecuteAsync()).Files;
        }
    }
}