namespace MediaPlaylistManager.DAL.EFCore.Sqlite.Entities;

public class PlaylistEntity
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public List<MediaItemEntity> MediaItems { get; set; } = [];
}