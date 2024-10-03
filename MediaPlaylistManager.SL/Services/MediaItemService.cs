using MediaPlaylistManager.BLL.Interfaces;
using MediaPlaylistManager.DTO.Models;
using MediaPlaylistManager.SL.Interfaces;
using MediaPlaylistManager.SL.Utils;

namespace MediaPlaylistManager.SL.Services;

public class MediaItemService : IMediaItemService
{
    private readonly IMediaItemManager _mediaItemManager;
    private readonly IFileService _fileService;

    public MediaItemService(
        IMediaItemManager mediaItemManager,
        IFileService fileService)
    {
        _mediaItemManager = mediaItemManager;
        _fileService = fileService;
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
        var mediaItem = await _mediaItemManager.GetMediaItemIdAsync(id);
        return mediaItem?.ToMediaItemDto();
    }

    public async Task<bool> UpdateMediaItemAsync(MediaItemDto mediaItemDto)
    {
        return await _mediaItemManager.UpdateMediaItemAsync(mediaItemDto.ToMediaItem());
    }

    public async Task<bool> DeleteMediaItemByIdAsync(int id)
    {
        var mediaItem = await GetMediaItemAsync(id);
        if (mediaItem is null)
            return false;

        if (!_fileService.DeleteFile(mediaItem.FilePath))
            return false;

        return await _mediaItemManager.DeleteMediaItemByIdAsync(id);
    }

    public async Task<List<MediaItemDto>> SearchMediaItemsAsync(string query)
    {
        var items = await _mediaItemManager.SearchMediaItemsAsync(query);
        return items
            .Select(mediaItem => mediaItem.ToMediaItemDto())
            .ToList();
    }
}