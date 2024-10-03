using MediaPlaylistManager.BLL.Interfaces;
using MediaPlaylistManager.BLL.Models;
using MediaPlaylistManager.BLL.Utils;
using MediaPlaylistManager.DAL.Shared.Interfaces;
using MediaPlaylistManager.DAL.Shared.Entities;

namespace MediaPlaylistManager.BLL.Managers;

public class PlaylistManager : IPlaylistManager
{
    private readonly IPlaylistRepository _playlistRepository;

    public PlaylistManager(IPlaylistRepository playlistRepository)
    {
        _playlistRepository = playlistRepository;
    }

    public async Task<Playlist> CreatePlaylistAsync(string title)
    {
        var entity = await _playlistRepository.CreatePlaylistAsync(title);
        return entity.ToPlaylist();
    }

    public async Task<List<Playlist>> GetPlaylistsAsync()
    {
        var entities = await _playlistRepository.GetPlaylistsAsync();
        return entities
            .Select(entity => entity.ToPlaylist())
            .ToList();
    }

    public async Task<Playlist?> GetPlaylistByIdAsync(int id)
    {
        var entity = await _playlistRepository.GetPlaylistByIdAsync(id);
        return entity?.ToPlaylist();
    }

    public async Task<bool> UpdatePlaylistAsync(Playlist playlist)
    {
        return await _playlistRepository.UpdatePlaylistAsync(
            playlist.ToPlaylistEntity<PlaylistEntity>());
    }

    public async Task<bool> DeletePlaylistByIdAsync(int id)
    {
        return await _playlistRepository.DeletePlaylistByIdAsync(id);
    }
}