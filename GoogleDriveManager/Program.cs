using GoogleDriveManager.Services;

class Program
{
    static async Task Main(string[] args)
    {
        var driveService = new GoogleDriveService();
        var files = await driveService.GetFilesAsync();

        Console.WriteLine("Google Drive files:");
        foreach (var file in files)
        {
            Console.WriteLine($"{file.Name} ({file.Id})");
        }
    }
}
