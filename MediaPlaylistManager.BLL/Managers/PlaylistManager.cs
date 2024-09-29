using MediaPlaylistManager.BLL.Interfaces;
using MediaPlaylistManager.BLL.Models;
using MediaPlaylistManager.BLL.Utils;
using MediaPlaylistManager.DAL.Interfaces;

namespace MediaPlaylistManager.BLL.Managers;

public class PlaylistManager : IPlaylistManager
{
    private readonly IPlaylistRepository _playlistRepository;

    public PlaylistManager(IPlaylistRepository playlistRepository)
    {
        _playlistRepository = playlistRepository;
    }

    public async Task<Playlist> AddPlaylistAsync(string title)
    {
        var entity = await _playlistRepository.AddPlaylistAsync(title);
        return entity.ToPlaylist();
    }

    public async Task<List<Playlist>> GetAllPlaylistsAsync()
    {
        var entities = await _playlistRepository.GetAllPlaylistsAsync();
        return entities
            .Select(entity => entity.ToPlaylist())
            .ToList();
    }

    public async Task<Playlist?> GetPlaylistAsync(int id)
    {
        var entity = await _playlistRepository.GetPlaylistAsync(id);
        return entity?.ToPlaylist();
    }

    public async Task<bool> UpdatePlaylistAsync(Playlist playlist)
    {
        return await _playlistRepository.UpdatePlaylistAsync(playlist.ToPlaylistEntity());
    }

    public async Task<bool> DeletePlaylistAsync(int id)
    {
        return await _playlistRepository.DeletePlaylistAsync(id);
    }
}