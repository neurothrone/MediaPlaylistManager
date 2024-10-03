using MediaPlaylistManager.DAL.Shared.Entities;

namespace MediaPlaylistManager.DAL.Shared.Interfaces;

public interface IPlaylistRepository
{
    Task<PlaylistEntity> CreatePlaylistAsync(string title);
    Task<List<PlaylistEntity>> GetPlaylistsAsync();
    Task<PlaylistEntity?> GetPlaylistByIdAsync(int id);
    Task<bool> UpdatePlaylistAsync(PlaylistEntity playlist);
    Task<bool> DeletePlaylistByIdAsync(int id);
}