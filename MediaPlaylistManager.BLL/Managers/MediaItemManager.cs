using MediaPlaylistManager.BLL.Interfaces;
using MediaPlaylistManager.BLL.Models;
using MediaPlaylistManager.BLL.Utils;
using MediaPlaylistManager.DAL.Shared.Interfaces;

namespace MediaPlaylistManager.BLL.Managers;

public class MediaItemManager : IMediaItemManager
{
    private readonly IMediaItemRepository _mediaItemRepository;

    public MediaItemManager(IMediaItemRepository mediaItemRepository)
    {
        _mediaItemRepository = mediaItemRepository;
    }

    public Task<int> CreateMediaItemAsync(MediaItem mediaItem)
    {
        return _mediaItemRepository.CreateMediaItemAsync(mediaItem.ToMediaItemEntity());
    }

    public async Task<List<MediaItem>> GetMediaItemsByPlaylistIdAsync(int playlistId)
    {
        var entities = await _mediaItemRepository
            .GetMediaItemsByPlaylistIdAsync(playlistId);
        return entities
            .Select(entity => entity.ToMediaItem())
            .ToList();
    }

    public async Task<MediaItem?> GetMediaItemIdAsync(int id)
    {
        var entity = await _mediaItemRepository.GetMediaItemByIdAsync(id);
        return entity?.ToMediaItem();
    }

    public async Task<bool> UpdateMediaItemAsync(MediaItem mediaItem)
    {
        return await _mediaItemRepository.UpdateMediaItemAsync(mediaItem.ToMediaItemEntity());
    }

    public async Task<bool> DeleteMediaItemByIdAsync(int id)
    {
        return await _mediaItemRepository.DeleteMediaItemByIdAsync(id);
    }

    public async Task<List<MediaItem>> SearchMediaItemsAsync(string query)
    {
        var results = await _mediaItemRepository
            .SearchMediaItemsAsync(query);
        return results
            .Select(entity => entity.ToMediaItem())
            .ToList();
    }
}