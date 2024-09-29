namespace MediaPlaylistManager.SL.Interfaces;

public interface IFileService
{
    Task<string?> PickFile();
}