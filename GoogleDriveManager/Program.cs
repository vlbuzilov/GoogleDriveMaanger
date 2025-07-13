using Google.Apis.Drive.v3;
using Google.Apis.Sheets.v4;
using Google.Apis.Services;
using GoogleDriveManager.Providers;
using GoogleDriveManager.Services;
using System;
using System.Threading.Tasks;
using GoogleDriveManager.Constants;

class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("Starting Google Drive to Sheets sync...");

        var credential = await GoogleCredentialProvider.GetCredentialAsync();

        var driveService = new DriveService(new BaseClientService.Initializer
        {
            HttpClientInitializer = credential,
            ApplicationName = Constants.ApplicationName
        });

        var sheetsService = new SheetsService(new BaseClientService.Initializer
        {
            HttpClientInitializer = credential,
            ApplicationName = Constants.ApplicationName
        });

        var googleDriveService = new GoogleDriveService(driveService);
        var googleSheetsService = new GoogleSheetsService(sheetsService, driveService);

        while (true)
        {
            try
            {
                Console.WriteLine($"{DateTime.Now}: Sync started...");

                var files = await googleDriveService.GetFilesAsync();

                var spreadsheetId = await googleSheetsService.GetOrCreateSpreadsheetAsync();

                await googleSheetsService.UpdateSpreadsheetAsync(spreadsheetId, files);

                Console.WriteLine($"{DateTime.Now}: Sync completed successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            Console.WriteLine("Waiting 15 minutes before next sync...\n");
            await Task.Delay(TimeSpan.FromMinutes(15));
        }
    }
}