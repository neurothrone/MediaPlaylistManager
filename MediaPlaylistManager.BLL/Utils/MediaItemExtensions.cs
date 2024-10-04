using MediaPlaylistManager.BLL.Models;
using MediaPlaylistManager.DAL.EFCore.Shared.Entities;

namespace MediaPlaylistManager.BLL.Utils;

internal static class MediaItemExtensions
{
    public static MediaItem ToMediaItem(this MediaItemEntity mediaItemEntityBase)
    {
        return new MediaItem
        {
            Id = mediaItemEntityBase.Id,
            PlaylistId = mediaItemEntityBase.PlaylistId,
            FilePath = mediaItemEntityBase.FilePath,
            Title = mediaItemEntityBase.Title,
            Artist = mediaItemEntityBase.Artist,
            Duration = mediaItemEntityBase.Duration
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