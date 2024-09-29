using MediaPlaylistManager.DAL.Entities;

namespace MediaPlaylistManager.DAL.Interfaces;

public interface IPlaylistRepository
{
    Task<PlaylistEntity> AddPlaylistAsync(string title);
    Task<List<PlaylistEntity>> GetAllPlaylistsAsync();
    Task<PlaylistEntity?> GetPlaylistAsync(int id);
    Task<bool> UpdatePlaylistAsync(PlaylistEntity playlist);
    Task<bool> DeletePlaylistAsync(int id);
}