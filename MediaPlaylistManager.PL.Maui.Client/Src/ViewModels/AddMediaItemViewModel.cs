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
    private readonly IDialogService _dialogService;
    private readonly IFileService _fileService;
    private readonly INavigator _navigator;
    private readonly IMediaItemService _mediaItemService;
    private readonly IMediaMetadataService _mediaMetadataService;

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
        IDialogService dialogService,
        IFileService fileService,
        INavigator navigator,
        IMediaItemService mediaItemService,
        IMediaMetadataService mediaMetadataService)
    {
        _dialogService = dialogService;
        _fileService = fileService;
        _navigator = navigator;
        _mediaItemService = mediaItemService;
        _mediaMetadataService = mediaMetadataService;
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

        string? errorMessage = null;

        try
        {
            IsLoading = true;

            int mediaItemId = await _mediaItemService.CreateMediaItemAsync(dto);

            await MainThread.InvokeOnMainThreadAsync(async () =>
            {
                IsLoading = false;

                if (mediaItemId == -1)
                {
                    errorMessage = "Something went wrong when creating media item";
                }
                else
                {
                    WeakReferenceMessenger.Default.Send(new MediaItemAddedMessage(PlaylistId, mediaItemId));
                    await _navigator.GoBackAsync();
                }
            });
        }
        catch (Exception ex)
        {
            IsLoading = false;
            errorMessage = $"Exception occurred: {ex.Message}";
        }
        finally
        {
            IsLoading = false;
        }

        if (errorMessage is not null)
            await _dialogService.ShowAlertAsync("Error", errorMessage, "OK");
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
            await _dialogService.ShowAlertAsync("Error", "Something went wrong when picking the file", "OK");
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