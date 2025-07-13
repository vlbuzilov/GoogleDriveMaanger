using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Drive.v3;
using Google.Apis.Services;

namespace GoogleDriveManager.Services
{
    public class GoogleSheetsService
    {
        private readonly SheetsService _sheetsService;
        private readonly DriveService _driveService;

        public GoogleSheetsService(SheetsService sheetsService, DriveService driveService)
        {
            _sheetsService = sheetsService;
            _driveService = driveService;
        }

        public async Task<string> GetOrCreateSpreadsheetAsync()
        {
            var listRequest = _driveService.Files.List();
            listRequest.Q = $"mimeType='application/vnd.google-apps.spreadsheet' and name='{Constants.Constants.SpreadsheetTitle}'";
            listRequest.Fields = "files(id, name)";
            var listResponse = await listRequest.ExecuteAsync();

            if (listResponse.Files != null && listResponse.Files.Count > 0)
            {
                return listResponse.Files[0].Id;
            }

            var spreadsheet = new Spreadsheet
            {
                Properties = new SpreadsheetProperties { Title = Constants.Constants.SpreadsheetTitle }
            };

            var createRequest = _sheetsService.Spreadsheets.Create(spreadsheet);
            var createResponse = await createRequest.ExecuteAsync();

            return createResponse.SpreadsheetId;
        }

        public async Task UpdateSpreadsheetAsync(string spreadsheetId, IEnumerable<Google.Apis.Drive.v3.Data.File> files)
        {
            var range = "Sheet1!A1";
            var values = new List<IList<object>>
            {
                new List<object> { "ID", "Name", "MimeType", "ModifiedTime" }
            };

            foreach (var file in files)
            {
                values.Add(new List<object>
                {
                    file.Id,
                    file.Name,
                    file.MimeType,
                    file.ModifiedTime?.ToString("g") ?? ""
                });
            }

            var valueRange = new ValueRange { Values = values };
            var updateRequest = _sheetsService.Spreadsheets.Values.Update(valueRange, spreadsheetId, range);
            updateRequest.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.RAW;
            await updateRequest.ExecuteAsync();
        }
    }
}