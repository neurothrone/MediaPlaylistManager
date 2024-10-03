using MediaPlaylistManager.BLL.Models;

namespace MediaPlaylistManager.BLL.Interfaces;

public interface IMediaItemManager
{
    Task<int> CreateMediaItemAsync(MediaItem mediaItem);
    Task<List<MediaItem>> GetMediaItemsByPlaylistIdAsync(int playlistId);
    Task<MediaItem?> GetMediaItemIdAsync(int id);
    Task<bool> UpdateMediaItemAsync(MediaItem mediaItem);
    Task<bool> DeleteMediaItemByIdAsync(int id);
    Task<List<MediaItem>> SearchMediaItemsAsync(string query);
}