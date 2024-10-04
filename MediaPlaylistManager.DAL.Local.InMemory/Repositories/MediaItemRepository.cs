using MediaPlaylistManager.DAL.Local.InMemory.Data;
using MediaPlaylistManager.DAL.Local.Shared.Entities;
using MediaPlaylistManager.DAL.Local.Shared.Interfaces;

namespace MediaPlaylistManager.DAL.Local.InMemory.Repositories;

public class MediaItemRepository : IMediaItemRepository
{
    private readonly DataStore _dataStore;
    private int _mediaItemsCount;

    public MediaItemRepository(DataStore dataStore)
    {
        _dataStore = dataStore;
    }

    public Task<int> CreateMediaItemAsync(MediaItemEntity mediaItem)
    {
        _mediaItemsCount++;

        mediaItem.Id = _mediaItemsCount;
        _dataStore.MediaItems.Add(mediaItem);

        return Task.FromResult(mediaItem.Id);
    }

    public Task<List<MediaItemEntity>> GetMediaItemsByPlaylistIdAsync(int playlistId)
    {
        var items = _dataStore.MediaItems
            .Where(m => m.PlaylistId == playlistId)
            .ToList();
        return Task.FromResult(items);
    }

    public Task<MediaItemEntity?> GetMediaItemByIdAsync(int id)
    {
        var mediaItem = _dataStore.MediaItems.FirstOrDefault(m => m.Id == id);
        return Task.FromResult(mediaItem);
    }

    public Task<bool> UpdateMediaItemAsync(MediaItemEntity mediaItem)
    {
        var mediaItemToUpdate = _dataStore.MediaItems.FirstOrDefault(e => e.Id == mediaItem.Id);
        if (mediaItemToUpdate == null)
            return Task.FromResult(false);

        mediaItemToUpdate.FilePath = mediaItem.FilePath;
        mediaItemToUpdate.Title = mediaItem.Title;
        mediaItemToUpdate.Artist = mediaItem.Artist;
        mediaItemToUpdate.Duration = mediaItem.Duration;
        return Task.FromResult(true);
    }

    public Task<bool> DeleteMediaItemByIdAsync(int id)
    {
        var mediaItemToDelete = _dataStore.MediaItems.FirstOrDefault(e => e.Id == id);
        if (mediaItemToDelete == null)
            return Task.FromResult(false);

        _dataStore.MediaItems.Remove(mediaItemToDelete);
        return Task.FromResult(true);
    }

    public Task<List<MediaItemEntity>> SearchMediaItemsAsync(string query)
    {
        var results = _dataStore.MediaItems
            .Where(m =>
                m.Title.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                m.Artist.Contains(query, StringComparison.OrdinalIgnoreCase)
            )
            .ToList();

        return Task.FromResult(results);
    }
}