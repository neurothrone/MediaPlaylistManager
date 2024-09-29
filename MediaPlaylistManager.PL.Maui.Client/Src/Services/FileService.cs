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

        return result?.FullPath;
    }
}