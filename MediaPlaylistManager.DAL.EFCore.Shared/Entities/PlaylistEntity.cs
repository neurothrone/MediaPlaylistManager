namespace MediaPlaylistManager.DAL.EFCore.Shared.Entities;

public class PlaylistEntity
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public ICollection<MediaItemEntity> MediaItems { get; set; } = [];
}