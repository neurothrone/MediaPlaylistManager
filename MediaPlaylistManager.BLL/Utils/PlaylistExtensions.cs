using MediaPlaylistManager.BLL.Models;
using MediaPlaylistManager.DAL.Shared.Entities;

namespace MediaPlaylistManager.BLL.Utils;

internal static class PlaylistExtensions
{
    public static Playlist ToPlaylist(this PlaylistEntity playlistEntityBase)
    {
        return new Playlist
        {
            Id = playlistEntityBase.Id,
            Title = playlistEntityBase.Title
        };
    }

    public static PlaylistEntity ToPlaylistEntity<T>(this Playlist playlist)
    {
        return new PlaylistEntity
        {
            Id = playlist.Id,
            Title = playlist.Title
        };
    }
}