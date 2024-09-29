using MediaPlaylistManager.SL.Domain;
using MediaPlaylistManager.SL.Interfaces;
using TagLib;

namespace MediaPlaylistManager.SL.Services;

public class MediaMetadataService : IMediaMetadataService
{
    // Source:
    // https://github.com/mono/taglib-sharp?tab=readme-ov-file#readme

    public MediaMetadata GetMediaMetadataAsync(string filePath)
    {
        var metadata = new MediaMetadata();

        if (!System.IO.File.Exists(filePath))
            return metadata;

        TagLib.File? file;

        try
        {
            file = TagLib.File.Create(filePath);
        }
        catch (Exception ex) when (ex is CorruptFileException or UnsupportedFormatException)
        {
            return metadata;
        }

        if (file == null)
            return metadata;

        // NOTE: FirstPerformer can be null, make sure to handle it.
        metadata.Title = file.Tag.Title;
        metadata.Artist = file.Tag.FirstPerformer;
        metadata.Duration = file.Properties.Duration;

        return metadata;
    }
}