using MediaPlaylistManager.SL.Interfaces;

namespace MediaPlaylistManager.PL.Maui.Client.Services;

public class FileService : IFileService
{
    // Source:
    // https://learn.microsoft.com/en-us/dotnet/maui/platform-integration/storage/file-picker?view=net-maui-8.0

    public async Task<string?> PickFile()
    {
        // UTI Reference Source:
        // https://developer.apple.com/library/archive/documentation/Miscellaneous/Reference/UTIRef/Articles/System-DeclaredUniformTypeIdentifiers.html

        var customAudioFileTypes = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
        {
            { DevicePlatform.iOS, ["public.audio"] }, // iOS-specific uniform type identifier (UTI)
            { DevicePlatform.Android, ["audio/*"] }, // Android MIME type
            { DevicePlatform.Tizen, ["*/*"] },
            { DevicePlatform.MacCatalyst, ["mp3", "mp4", "wav"] }, // macOS file extensions
            { DevicePlatform.WinUI, [".mp3", "mp4", ".wav", ".wma"] } // Windows file extensions
        });

        FileResult? result = null;
        try
        {
            result = await FilePicker.PickAsync(
                new PickOptions
                {
                    PickerTitle = "Pick Audio File",
                    FileTypes = customAudioFileTypes
                }
            );
        }
        catch (Exception ex)
        {
            // Occurs when something goes wrong or when the user canceled the file picking.
            Console.WriteLine($"Error selecting file: {ex.Message}");
        }

        return result is null ? null : SaveFile(result.FullPath);
    }

    public string? SaveFile(string sourceFilePath)
    {
        try
        {
            var fileName = Path.GetFileName(sourceFilePath);
            var mediaItemsDirectory = Path.Combine(FileSystem.AppDataDirectory, "MediaItems");

            if (!Directory.Exists(mediaItemsDirectory))
                Directory.CreateDirectory(mediaItemsDirectory);

            var destinationPath = Path.Combine(mediaItemsDirectory, fileName);
            if (!File.Exists(destinationPath))
                File.Copy(sourceFilePath, destinationPath, true);

            return destinationPath;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error copying file: {ex}");
            return null;
        }
    }

    public bool DeleteFile(string sourceFilePath)
    {
        try
        {
            if (File.Exists(sourceFilePath))
                File.Delete(sourceFilePath);

            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error deleting file: {ex}");
            return false;
        }
    }
}