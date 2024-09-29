using MediaPlaylistManager.DTO.Models;
using MediaPlaylistManager.PL.Maui.Client.ViewModels;

namespace MediaPlaylistManager.PL.Maui.Client.Utils;

internal static class MediaItemExtensions
{
    public static MediaItemDto ToMediaItemDto(this MediaItemViewModel viewModel)
    {
        return new MediaItemDto(
            viewModel.Id,
            viewModel.PlaylistId,
            viewModel.FilePath,
            viewModel.Title,
            viewModel.Artist,
            viewModel.Duration
        );
    }

    public static MediaItemViewModel ToMediaItemViewModel(this MediaItemDto dto)
    {
        return new MediaItemViewModel
        {
            Id = dto.Id,
            PlaylistId = dto.PlaylistId,
            FilePath = dto.FilePath,
            Title = dto.Title,
            Artist = dto.Artist,
            Duration = dto.Duration
        };
    }
}