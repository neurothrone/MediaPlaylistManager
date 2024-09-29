using MediaPlaylistManager.DAL.Entities;

namespace MediaPlaylistManager.DAL.Data;

public class DataStore
{
    public readonly List<PlaylistEntity> Playlists = [];
    public readonly List<MediaItemEntity> MediaItems = [];
}