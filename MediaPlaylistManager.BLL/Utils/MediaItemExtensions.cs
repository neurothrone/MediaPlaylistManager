using MediaPlaylistManager.BLL.Models;
using MediaPlaylistManager.DAL.Entities;

namespace MediaPlaylistManager.BLL.Utils;

internal static class MediaItemExtensions
{
    public static MediaItem ToMediaItem(this MediaItemEntity mediaItemEntity)
    {
        return new MediaItem
        {
            Id = mediaItemEntity.Id,
            PlaylistId = mediaItemEntity.PlaylistId,
            FilePath = mediaItemEntity.FilePath,
            Title = mediaItemEntity.Title,
            Artist = mediaItemEntity.Artist,
            Duration = mediaItemEntity.Duration
        };
    }

    public static MediaItemEntity ToMediaItemEntity(this MediaItem mediaItem)
    {
        return new MediaItemEntity
        {
            Id = mediaItem.Id,
            PlaylistId = mediaItem.PlaylistId,
            FilePath = mediaItem.FilePath,
            Title = mediaItem.Title,
            Artist = mediaItem.Artist,
            Duration = mediaItem.Duration
        };
    }
}