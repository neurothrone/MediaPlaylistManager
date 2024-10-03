using SQLite;

namespace MediaPlaylistManager.DAL.Shared.Entities;

public class PlaylistEntity
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;
}