using MediaPlaylistManager.DTO.Models;
using MediaPlaylistManager.PL.Maui.Client.ViewModels;

namespace MediaPlaylistManager.PL.Maui.Client.Utils;

internal static class PlaylistExtensions
{
    public static PlaylistDto ToPlaylistDto(this PlaylistViewModel viewModel)
    {
        return new PlaylistDto(
            viewModel.Id,
            viewModel.Title
        );
    }

    public static PlaylistViewModel ToPlaylistViewModel(this PlaylistDto dto)
    {
        return new PlaylistViewModel
        {
            Id = dto.Id,
            Title = dto.Title
        };
    }
}