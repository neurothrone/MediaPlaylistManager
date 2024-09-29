using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using MediaPlaylistManager.PL.Maui.Client.Enums;
using MediaPlaylistManager.PL.Maui.Client.Messages;
using MediaPlaylistManager.PL.Maui.Client.Utils;
using MediaPlaylistManager.SL.Interfaces;

namespace MediaPlaylistManager.PL.Maui.Client.ViewModels;

public partial class PlaylistDetailViewModel :
    MediaItemListViewModel,
    IQueryAttributable,
    IRecipient<MediaItemAddedMessage>
{
    private readonly IPlaylistService _playlistService;

    [ObservableProperty]
    private int _playlistId;

    [ObservableProperty]
    private PlaylistViewModel? _playlist;

    [ObservableProperty]
    private bool _isLoading;

    private bool _isFirstAppearance = true;

    public PlaylistDetailViewModel(
        IPlaylistService playlistService,
        IMediaItemService mediaItemService,
        INavigator navigator) : base(mediaItemService, navigator)
    {
        _playlistService = playlistService;

        WeakReferenceMessenger.Default.Register<MediaItemAddedMessage>(this);
    }

    ~PlaylistDetailViewModel() => WeakReferenceMessenger.Default.Unregister<MediaItemAddedMessage>(this);

    [RelayCommand]
    private async Task NavigateToAddMediaItemPage()
    {
        await Navigator.GoToAsync(
            $"{nameof(AppRoute.AddMediaItem)}?{nameof(MediaItemViewModel.PlaylistId)}={PlaylistId}");
    }

    public async Task OnAppearing()
    {
        if (!_isFirstAppearance)
            return;

        await GetMediaItemsAsync();
        _isFirstAppearance = false;
    }

    private async Task AddMediaItemAsync(int mediaItemId)
    {
        if (MediaItems.Any(m => m.Id == mediaItemId))
            return;

        var mediaItemDto = await MediaItemService.GetMediaItemAsync(mediaItemId);
        if (mediaItemDto is null)
            return;

        var mediaItem = mediaItemDto.ToMediaItemViewModel();
        MainThread.BeginInvokeOnMainThread(() => MediaItems.Add(mediaItem));
    }

    private async Task GetMediaItemsAsync()
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            MediaItems.Clear();
            IsLoading = true;
        });

        var mediaItems = await MediaItemService.GetMediaItemsByPlaylistIdAsync(PlaylistId);

        MainThread.BeginInvokeOnMainThread(() =>
        {
            IsLoading = false;
            foreach (var mediaItemDto in mediaItems)
            {
                MediaItems.Add(mediaItemDto.ToMediaItemViewModel());
            }
        });
    }

    private async void LoadPlaylistDetailsByIdAsync(int id)
    {
        PlaylistId = id;

        var playlistDto = await _playlistService.GetPlaylistByIdAsync(PlaylistId);
        if (playlistDto is null)
            return;

        MainThread.BeginInvokeOnMainThread(() => Playlist = playlistDto.ToPlaylistViewModel());
    }

    #region IQueryAttributable

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (!query.TryGetValue(nameof(PlaylistViewModel.Id), out var idValue)
            || idValue is not string stringId || !int.TryParse(stringId, out var playlistId))
            return;

        LoadPlaylistDetailsByIdAsync(playlistId);
    }

    #endregion

    #region IRecipient<MediaItemAddedMessage>

    public async void Receive(MediaItemAddedMessage message)
    {
        if (message.PlaylistId != PlaylistId)
            return;

        await AddMediaItemAsync(message.MediaItemId);
    }

    #endregion
}