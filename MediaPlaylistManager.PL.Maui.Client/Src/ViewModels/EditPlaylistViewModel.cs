using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using MediaPlaylistManager.DTO.Models;
using MediaPlaylistManager.PL.Maui.Client.Messages;
using MediaPlaylistManager.PL.Maui.Client.Utils;
using MediaPlaylistManager.SL.Interfaces;

namespace MediaPlaylistManager.PL.Maui.Client.ViewModels;

public partial class EditPlaylistViewModel : ObservableObject, IQueryAttributable
{
    private readonly IPlaylistService _playlistService;
    private readonly INavigator _navigator;

    [ObservableProperty]
    private int _playlistId;

    [ObservableProperty]
    private PlaylistViewModel? _playlist;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(SaveChangesCommand))]
    private string? _title;

    public EditPlaylistViewModel(
        IPlaylistService playlistService,
        INavigator navigator)
    {
        _playlistService = playlistService;
        _navigator = navigator;
    }

    private bool CanSaveChanges() =>
        Playlist is not null &&
        Title is not null &&
        !string.IsNullOrWhiteSpace(Title) &&
        !Title.Equals(Playlist?.Title);

    [RelayCommand(CanExecute = nameof(CanSaveChanges))]
    private async Task SaveChanges()
    {
        if (string.IsNullOrWhiteSpace(Title))
            return;

        var playlistDto = new PlaylistDto(PlaylistId, Title);
        if (!await _playlistService.UpdatePlaylistAsync(playlistDto))
            return;

        WeakReferenceMessenger.Default.Send(new PlaylistUpdatedMessage(playlistDto.ToPlaylistViewModel()));
        await _navigator.GoBackAsync();
    }

    private async void LoadPlaylistDetailsByIdAsync(int id)
    {
        var playlistDto = await _playlistService.GetPlaylistByIdAsync(id);
        if (playlistDto is null)
            return;

        MainThread.BeginInvokeOnMainThread(() =>
        {
            Playlist = playlistDto.ToPlaylistViewModel();
            PlaylistId = id;
            Title = playlistDto.Title;
        });
    }

    #region IQueryAttributable

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (!query.TryGetValue(nameof(PlaylistViewModel.Id), out var idValue) ||
            idValue is not string stringId ||
            !int.TryParse(stringId, out var playlistId))
            return;

        LoadPlaylistDetailsByIdAsync(playlistId);
    }

    #endregion
}