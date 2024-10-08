using MediaPlaylistManager.DAL.EFCore.Shared.Entities;

namespace MediaPlaylistManager.DAL.EFCore.Shared.Interfaces;

public interface IPlaylistRepository
{
    Task<PlaylistEntity> CreatePlaylistAsync(string title);
    Task<IReadOnlyCollection<PlaylistEntity>> GetPlaylistsAsync();
    Task<PlaylistEntity?> GetPlaylistByIdAsync(int id);
    Task<bool> UpdatePlaylistAsync(PlaylistEntity playlist);
    Task<bool> DeletePlaylistByIdAsync(int id);
}