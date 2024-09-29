using MediaPlaylistManager.SL.Domain;

namespace MediaPlaylistManager.SL.Interfaces;

public interface IMediaMetadataService
{
    MediaMetadata GetMediaMetadataAsync(string filePath);
}