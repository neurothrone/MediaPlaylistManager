using MediaPlaylistManager.DTO.Models;

namespace MediaPlaylistManager.SL.Interfaces;

public interface IMediaItemService
{
    Task<int> CreateMediaItemAsync(MediaItemDto mediaItem);
    Task<List<MediaItemDto>> GetMediaItemsByPlaylistIdAsync(int playlistId);
    Task<MediaItemDto?> GetMediaItemAsync(int id);
    Task<bool> UpdateMediaItemAsync(MediaItemDto mediaItem);
    Task<bool> DeleteMediaItemAsync(int id);
    Task<List<MediaItemDto>> SearchMediaItemsAsync(string query);
}