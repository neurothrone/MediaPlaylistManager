using MediaPlaylistManager.DAL.Entities;

namespace MediaPlaylistManager.DAL.Interfaces;

public interface IMediaItemRepository
{
    Task<int> CreateMediaItemAsync(MediaItemEntity mediaItem);
    Task<List<MediaItemEntity>> GetMediaItemsByPlaylistIdAsync(int playlistId);
    Task<MediaItemEntity?> GetMediaItemAsync(int id);
    Task<bool> UpdateMediaItemAsync(MediaItemEntity mediaItem);
    Task<bool> DeleteMediaItemAsync(int id);
    Task<List<MediaItemEntity>> SearchMediaItemsAsync(string query);
}