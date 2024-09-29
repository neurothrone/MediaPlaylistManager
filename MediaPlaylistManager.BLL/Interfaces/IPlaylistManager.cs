using MediaPlaylistManager.BLL.Models;

namespace MediaPlaylistManager.BLL.Interfaces;

public interface IPlaylistManager
{
    Task<Playlist> AddPlaylistAsync(string title);
    Task<List<Playlist>> GetAllPlaylistsAsync();
    Task<Playlist?> GetPlaylistAsync(int id);
    Task<bool> UpdatePlaylistAsync(Playlist playlist);
    Task<bool> DeletePlaylistAsync(int id);
}