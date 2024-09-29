using MediaPlaylistManager.BLL.Models;
using MediaPlaylistManager.DTO;
using MediaPlaylistManager.DTO.Models;

namespace MediaPlaylistManager.SL.Utils;

public static class PlaylistExtensions
{
    public static Playlist ToPlaylist(this PlaylistDto playlistDto)
    {
        return new Playlist
        {
            Id = playlistDto.Id,
            Title = playlistDto.Title
        };
    }

    public static PlaylistDto ToPlaylistDto(this Playlist playlist)
    {
        return new PlaylistDto(
            playlist.Id,
            playlist.Title
        );
    }
}