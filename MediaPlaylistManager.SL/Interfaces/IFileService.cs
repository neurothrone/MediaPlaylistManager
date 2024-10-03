namespace MediaPlaylistManager.SL.Interfaces;

public interface IFileService
{
    Task<string?> PickFile();
    string? SaveFile(string sourceFilePath);
    bool DeleteFile(string sourceFilePath);
}