using MediaPlaylistManager.BLL.Models;

namespace MediaPlaylistManager.BLL.Interfaces;

public interface IPlaylistManager
{
    Task<Playlist> CreatePlaylistAsync(string title);
    Task<List<Playlist>> GetPlaylistsAsync();
    Task<Playlist?> GetPlaylistByIdAsync(int id);
    Task<bool> UpdatePlaylistAsync(Playlist playlist);
    Task<bool> DeletePlaylistByIdAsync(int id);
}