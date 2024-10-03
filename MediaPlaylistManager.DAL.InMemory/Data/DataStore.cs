using MediaPlaylistManager.DAL.Shared.Entities;

namespace MediaPlaylistManager.DAL.InMemory.Data;

public class DataStore
{
    public readonly List<PlaylistEntity> Playlists = [];
    public readonly List<MediaItemEntity> MediaItems = [];
}