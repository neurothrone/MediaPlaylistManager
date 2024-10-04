using MediaPlaylistManager.BLL.Models;
using MediaPlaylistManager.DTO.Models;

namespace MediaPlaylistManager.SL.Utils;

internal static class MediaItemExtensions
{
    public static MediaItem ToMediaItem(this MediaItemDto mediaItemDto)
    {
        return new MediaItem
        {
            Id = mediaItemDto.Id,
            PlaylistId = mediaItemDto.PlaylistId,
            FilePath = mediaItemDto.FilePath,
            Title = mediaItemDto.Title,
            Artist = mediaItemDto.Artist,
            Duration = mediaItemDto.Duration
        };
    }

    public static MediaItemDto ToMediaItemDto(this MediaItem mediaItem)
    {
        return new MediaItemDto(
            mediaItem.Id,
            mediaItem.PlaylistId,
            mediaItem.FilePath,
            mediaItem.Title,
            mediaItem.Artist,
            mediaItem.Duration
        );
    }
}