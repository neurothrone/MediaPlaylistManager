using MediaPlaylistManager.DAL.EFCore.Shared.Entities;

namespace MediaPlaylistManager.DAL.EFCore.Shared.Interfaces;

public interface IMediaItemRepository
{
    Task<int> CreateMediaItemAsync(MediaItemEntity mediaItem);
    Task<IReadOnlyCollection<MediaItemEntity>> GetMediaItemsByPlaylistIdAsync(int playlistId);
    Task<MediaItemEntity?> GetMediaItemByIdAsync(int id);
    Task<bool> UpdateMediaItemAsync(MediaItemEntity mediaItem);
    Task<bool> DeleteMediaItemByIdAsync(int id);
    Task<IReadOnlyCollection<MediaItemEntity>> SearchMediaItemsAsync(string query);
}