using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using MediaPlaylistManager.PL.Maui.Client.Messages;
using MediaPlaylistManager.PL.Maui.Client.Utils;
using MediaPlaylistManager.SL.Interfaces;

namespace MediaPlaylistManager.PL.Maui.Client.ViewModels;

public partial class EditMediaItemViewModel :
    ObservableObject,
    IQueryAttributable
{
    private readonly IMediaItemService _mediaItemService;
    private readonly INavigator _navigator;

    private MediaItemViewModel? _mediaItem;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(SaveChangesCommand))]
    private string _title = string.Empty;

    [ObservableProperty]
    private string _artist = string.Empty;

    [ObservableProperty]
    private TimeSpan _duration;

    public EditMediaItemViewModel(
        IMediaItemService mediaItemService,
        INavigator navigator)
    {
        _mediaItemService = mediaItemService;
        _navigator = navigator;
    }

    private bool CanSaveMediaItem() => _mediaItem is not null &&
                                       !string.IsNullOrWhiteSpace(_mediaItem.FilePath) &&
                                       !string.IsNullOrWhiteSpace(Title);

    [RelayCommand(CanExecute = nameof(CanSaveMediaItem))]
    private async Task SaveChanges()
    {
        if (_mediaItem is null ||
            string.IsNullOrWhiteSpace(_mediaItem.FilePath) ||
            string.IsNullOrWhiteSpace(Title))
            return;

        _mediaItem.Title = Title;
        _mediaItem.Artist = Artist;

        WeakReferenceMessenger.Default.Send(new MediaItemUpdatedMessage(_mediaItem));
        await _navigator.GoBackAsync();
    }

    private async Task LoadMediaItemDetailsByIdAsync(int id)
    {
        var mediaItemDto = await _mediaItemService.GetMediaItemAsync(id);
        if (mediaItemDto is null)
            return;

        _mediaItem = mediaItemDto.ToMediaItemViewModel();

        MainThread.BeginInvokeOnMainThread(() =>
        {
            Title = mediaItemDto.Title;
            Artist = mediaItemDto.Artist;
            Duration = mediaItemDto.Duration;
        });
    }

    #region IQueryAttributable

    public async void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (!query.TryGetValue(nameof(MediaItemViewModel.Id), out var idValue)
            || idValue is not string stringId || !int.TryParse(stringId, out var mediaItemId))
            return;

        await LoadMediaItemDetailsByIdAsync(mediaItemId);
    }

    #endregion
}