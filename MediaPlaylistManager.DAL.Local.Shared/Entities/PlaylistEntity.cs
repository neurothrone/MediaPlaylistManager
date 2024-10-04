using SQLite;

namespace MediaPlaylistManager.DAL.Local.Shared.Entities;

public class PlaylistEntity
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;
}