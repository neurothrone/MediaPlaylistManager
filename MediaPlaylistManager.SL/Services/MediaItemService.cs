using MediaPlaylistManager.BLL.Interfaces;
using MediaPlaylistManager.DTO.Models;
using MediaPlaylistManager.SL.Interfaces;
using MediaPlaylistManager.SL.Utils;

namespace MediaPlaylistManager.SL.Services;

public class MediaItemService : IMediaItemService
{
    private readonly IMediaItemManager _mediaItemManager;

    public MediaItemService(IMediaItemManager mediaItemManager)
    {
        _mediaItemManager = mediaItemManager;
    }

    public async Task<int> CreateMediaItemAsync(MediaItemDto mediaItemDto)
    {
        return await _mediaItemManager.CreateMediaItemAsync(mediaItemDto.ToMediaItem());
    }

    public async Task<List<MediaItemDto>> GetMediaItemsByPlaylistIdAsync(int playlistId)
    {
        var items = await _mediaItemManager.GetMediaItemsByPlaylistIdAsync(playlistId);
        return items
            .Select(mediaItem => mediaItem.ToMediaItemDto())
            .ToList();
    }

    public async Task<MediaItemDto?> GetMediaItemAsync(int id)
    {
        var mediaItem = await _mediaItemManager.GetMediaItemAsync(id);
        return mediaItem?.ToMediaItemDto();
    }

    public async Task<bool> UpdateMediaItemAsync(MediaItemDto mediaItemDto)
    {
        return await _mediaItemManager.UpdateMediaItemAsync(mediaItemDto.ToMediaItem());
    }

    public async Task<bool> DeleteMediaItemAsync(int id)
    {
        return await _mediaItemManager.DeleteMediaItemAsync(id);
    }

    public async Task<List<MediaItemDto>> SearchMediaItemsAsync(string query)
    {
        var items = await _mediaItemManager.SearchMediaItemsAsync(query);
        return items
            .Select(mediaItem => mediaItem.ToMediaItemDto())
            .ToList();
    }
}