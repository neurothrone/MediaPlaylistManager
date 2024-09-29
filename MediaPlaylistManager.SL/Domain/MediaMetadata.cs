namespace MediaPlaylistManager.SL.Domain;

public class MediaMetadata
{
    public string Title { get; set; } = string.Empty;
    public string Artist { get; set; } = string.Empty;
    public TimeSpan Duration { get; set; }
}