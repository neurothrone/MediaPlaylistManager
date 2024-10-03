using MediaPlaylistManager.DAL.EFCore.Sqlite.Entities;

namespace MediaPlaylistManager.DAL.EFCore.Sqlite.Interfaces;

public interface IMediaItemRepository
{
    Task<int> CreateMediaItemAsync(MediaItemEntity mediaItem);
    Task<List<MediaItemEntity>> GetMediaItemsByPlaylistIdAsync(int playlistId);
    Task<MediaItemEntity?> GetMediaItemByIdAsync(int id);
    Task<bool> UpdateMediaItemAsync(MediaItemEntity mediaItem);
    Task<bool> DeleteMediaItemByIdAsync(int id);
    Task<List<MediaItemEntity>> SearchMediaItemsAsync(string query);
}