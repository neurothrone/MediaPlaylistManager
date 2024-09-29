using MediaPlaylistManager.DAL.Data;
using MediaPlaylistManager.DAL.Entities;
using MediaPlaylistManager.DAL.Interfaces;

namespace MediaPlaylistManager.DAL.Repositories;

public class PlaylistRepository : IPlaylistRepository
{
    private readonly DataStore _dataStore;
    private int _playlistsCount;

    public PlaylistRepository(DataStore dataStore)
    {
        _dataStore = dataStore;
    }

    public Task<PlaylistEntity> AddPlaylistAsync(string title)
    {
        _playlistsCount++;

        var playlist = new PlaylistEntity { Id = _playlistsCount, Title = title };
        _dataStore.Playlists.Add(playlist);

        return Task.FromResult(playlist);
    }

    public Task<List<PlaylistEntity>> GetAllPlaylistsAsync()
    {
        return Task.FromResult(_dataStore.Playlists);
    }

    public Task<PlaylistEntity?> GetPlaylistAsync(int id)
    {
        var playlist = _dataStore.Playlists.FirstOrDefault(p => p.Id == id);
        return Task.FromResult(playlist);
    }

    public Task<bool> UpdatePlaylistAsync(PlaylistEntity playlist)
    {
        var playlistToUpdate = _dataStore.Playlists.FirstOrDefault(p => p.Id == playlist.Id);
        if (playlistToUpdate == null)
            return Task.FromResult(false);

        playlistToUpdate.Title = playlist.Title;
        return Task.FromResult(true);
    }

    public Task<bool> DeletePlaylistAsync(int id)
    {
        var playlistToDelete = _dataStore.Playlists.FirstOrDefault(p => p.Id == id);
        if (playlistToDelete == null)
            return Task.FromResult(false);

        _dataStore.Playlists.Remove(playlistToDelete);
        return Task.FromResult(true);
    }
}