namespace MediaPlaylistManager.BLL.Models;

public class MediaItem
{
    public int Id { get; set; }
    public int PlaylistId { get; set; }
    public string FilePath { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Artist { get; set; } = string.Empty;
    public TimeSpan Duration { get; set; }
}