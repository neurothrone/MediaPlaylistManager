using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using MediaPlaylistManager.DTO.Models;
using MediaPlaylistManager.PL.Maui.Client.Messages;
using MediaPlaylistManager.SL.Interfaces;

namespace MediaPlaylistManager.PL.Maui.Client.ViewModels;

public partial class AddMediaItemViewModel :
    ObservableObject,
    IQueryAttributable
{
    private readonly IMediaItemService _mediaItemService;
    private readonly IFileService _fileService;
    private readonly IMediaMetadataService _mediaMetadataService;
    private readonly INavigator _navigator;

    [ObservableProperty]
    private int _playlistId;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(AddMediaItemCommand))]
    private string _filePath = string.Empty;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(AddMediaItemCommand))]
    private string _title = string.Empty;

    [ObservableProperty]
    private string _artist = string.Empty;

    [ObservableProperty]
    private TimeSpan _duration;

    [ObservableProperty]
    private bool _isLoading;

    public AddMediaItemViewModel(
        IMediaItemService mediaItemService,
        IFileService fileService,
        IMediaMetadataService mediaMetadataService,
        INavigator navigator)
    {
        _mediaItemService = mediaItemService;
        _fileService = fileService;
        _mediaMetadataService = mediaMetadataService;
        _navigator = navigator;
    }

    private bool CanSaveMediaItem() => !string.IsNullOrWhiteSpace(FilePath) &&
                                       !string.IsNullOrWhiteSpace(Title);

    [RelayCommand(CanExecute = nameof(CanSaveMediaItem))]
    private async Task AddMediaItem()
    {
        if (string.IsNullOrWhiteSpace(FilePath) || string.IsNullOrWhiteSpace(Title))
            return;

        var dto = new MediaItemDto(
            -1,
            PlaylistId,
            FilePath,
            Title,
            Artist,
            Duration);

        int mediaItemId = await _mediaItemService.CreateMediaItemAsync(dto);
        if (mediaItemId == -1)
        {
            // TODO: Show error message
            return;
        }

        WeakReferenceMessenger.Default.Send(new MediaItemAddedMessage(PlaylistId, mediaItemId));
        await _navigator.GoBackAsync();
    }

    [RelayCommand]
    private async Task PickFile()
    {
        MainThread.BeginInvokeOnMainThread(() => IsLoading = true);

        try
        {
            string? filePath = await _fileService.PickFile();
            if (filePath is null)
                return;

            var metadata = _mediaMetadataService.GetMediaMetadataAsync(filePath);

            MainThread.BeginInvokeOnMainThread(() =>
            {
                FilePath = filePath;
                Title = !string.IsNullOrWhiteSpace(metadata.Title) ? metadata.Title : string.Empty;
                Artist = !string.IsNullOrWhiteSpace(metadata.Artist) ? metadata.Artist : string.Empty;
                Duration = metadata.Duration;
            });
        }
        catch
        {
            // TODO: Show feedback?
            // MainThread.BeginInvokeOnMainThread(() => ErrorMessage = "No file selected");
        }
        finally
        {
            MainThread.BeginInvokeOnMainThread(() => IsLoading = false);
        }
    }

    #region IQueryAttributable

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (!query.TryGetValue(nameof(MediaItemViewModel.PlaylistId), out var idValue)
            || idValue is not string stringId || !int.TryParse(stringId, out var playlistId))
            return;

        MainThread.BeginInvokeOnMainThread(() => PlaylistId = playlistId);
    }

    #endregion
}