using MediaPlaylistManager.BLL.Interfaces;
using MediaPlaylistManager.DTO.Models;
using MediaPlaylistManager.SL.Interfaces;
using MediaPlaylistManager.SL.Utils;

namespace MediaPlaylistManager.SL.Services;

public class PlaylistService : IPlaylistService
{
    private readonly IPlaylistManager _playlistManager;

    public PlaylistService(IPlaylistManager playlistManager)
    {
        _playlistManager = playlistManager;
    }

    public async Task<PlaylistDto> CreatePlaylistAsync(string title)
    {
        var playlist = await _playlistManager.CreatePlaylistAsync(title);
        return playlist.ToPlaylistDto();
    }

    public async Task<List<PlaylistDto>> GetPlaylistsAsync()
    {
        var playlists = await _playlistManager.GetPlaylistsAsync();
        return playlists
            .Select(playlist => playlist.ToPlaylistDto())
            .ToList();
    }

    public async Task<PlaylistDto?> GetPlaylistByIdAsync(int id)
    {
        var playlist = await _playlistManager.GetPlaylistByIdAsync(id);
        return playlist?.ToPlaylistDto();
    }

    public async Task<bool> UpdatePlaylistAsync(PlaylistDto playlistDto)
    {
        return await _playlistManager.UpdatePlaylistAsync(playlistDto.ToPlaylist());
    }

    public async Task<bool> DeletePlaylistByIdAsync(int id)
    {
        return await _playlistManager.DeletePlaylistByIdAsync(id);
    }
}