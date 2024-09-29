using MediaPlaylistManager.BLL.Models;
using MediaPlaylistManager.DAL.Entities;

namespace MediaPlaylistManager.BLL.Utils;

internal static class PlaylistExtensions
{
    public static Playlist ToPlaylist(this PlaylistEntity playlistEntity)
    {
        return new Playlist
        {
            Id = playlistEntity.Id,
            Title = playlistEntity.Title
        };
    }

    public static PlaylistEntity ToPlaylistEntity(this Playlist playlist)
    {
        return new PlaylistEntity
        {
            Id = playlist.Id,
            Title = playlist.Title
        };
    }
}