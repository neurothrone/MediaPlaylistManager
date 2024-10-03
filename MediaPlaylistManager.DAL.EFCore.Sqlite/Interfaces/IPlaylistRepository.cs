using MediaPlaylistManager.DAL.EFCore.Sqlite.Entities;

namespace MediaPlaylistManager.DAL.EFCore.Sqlite.Interfaces;

public interface IPlaylistRepository
{
    Task<PlaylistEntity> CreatePlaylistAsync(string title);
    Task<List<PlaylistEntity>> GetPlaylistsAsync();
    Task<PlaylistEntity?> GetPlaylistByIdAsync(int id);
    Task<bool> UpdatePlaylistAsync(PlaylistEntity playlist);
    Task<bool> DeletePlaylistByIdAsync(int id);
}