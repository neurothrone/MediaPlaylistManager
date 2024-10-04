using MediaPlaylistManager.DAL.Local.Shared.Entities;

namespace MediaPlaylistManager.DAL.Local.InMemory.Data;

public class DataStore
{
    public readonly List<PlaylistEntity> Playlists = [];
    public readonly List<MediaItemEntity> MediaItems = [];
}