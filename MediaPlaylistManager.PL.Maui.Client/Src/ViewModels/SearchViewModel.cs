using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MediaPlaylistManager.DTO.Models;
using MediaPlaylistManager.PL.Maui.Client.Utils;
using MediaPlaylistManager.SL.Interfaces;

namespace MediaPlaylistManager.PL.Maui.Client.ViewModels;

public partial class SearchViewModel : MediaItemListViewModel
{
    [ObservableProperty]
    private string _searchText = string.Empty;

    [ObservableProperty]
    private string _searchResultsFeedback = string.Empty;

    public SearchViewModel(
        IMediaItemService mediaItemService,
        INavigator navigator) : base(mediaItemService, navigator)
    {
    }

    [RelayCommand]
    private async Task Search()
    {
        if (string.IsNullOrWhiteSpace(SearchText))
        {
            MainThread.BeginInvokeOnMainThread(() => SearchResultsFeedback = "Try entering search text");
            return;
        }

        List<MediaItemDto> searchResults = await MediaItemService.SearchMediaItemsAsync(SearchText);

        MainThread.BeginInvokeOnMainThread(() =>
        {
            SearchResultsFeedback = searchResults.Count == 0 ? "No media items found" : string.Empty;

            if (MediaItems.Count != 0)
                MediaItems.Clear();

            if (searchResults.Count == 0)
                return;

            foreach (var mediaItemDto in searchResults)
            {
                MediaItems.Add(mediaItemDto.ToMediaItemViewModel());
            }
        });
    }
}