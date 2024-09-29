using MediaPlaylistManager.DTO.Models;

namespace MediaPlaylistManager.SL.Interfaces;

public interface IPlaylistService
{
    Task<PlaylistDto> CreatePlaylistAsync(string title);
    Task<List<PlaylistDto>> GetPlaylistsAsync();
    Task<PlaylistDto?> GetPlaylistByIdAsync(int id);
    Task<bool> UpdatePlaylistAsync(PlaylistDto playlist);
    Task<bool> DeletePlaylistByIdAsync(int id);
}