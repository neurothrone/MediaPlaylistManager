using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using MediaPlaylistManager.PL.Maui.Client.Enums;
using MediaPlaylistManager.PL.Maui.Client.Messages;
using MediaPlaylistManager.PL.Maui.Client.Utils;
using MediaPlaylistManager.SL.Interfaces;

namespace MediaPlaylistManager.PL.Maui.Client.ViewModels;

public partial class PlaylistListViewModel :
    ObservableObject,
    IRecipient<PlaylistUpdatedMessage>
{
    private readonly IPlaylistService _playlistService;
    private readonly INavigator _navigator;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(AddPlaylistCommand))]
    private string _title = string.Empty;

    public ObservableCollection<PlaylistViewModel> Playlists { get; } = [];

    public PlaylistListViewModel(
        IPlaylistService playlistService,
        INavigator navigator)
    {
        _playlistService = playlistService;
        _navigator = navigator;

        WeakReferenceMessenger.Default.Register(this);
    }

    ~PlaylistListViewModel() => WeakReferenceMessenger.Default.Unregister<PlaylistUpdatedMessage>(this);

    private bool CanAddPlaylist() => !string.IsNullOrWhiteSpace(Title);

    [RelayCommand(CanExecute = nameof(CanAddPlaylist))]
    private async Task AddPlaylist()
    {
        var playlist = await _playlistService.CreatePlaylistAsync(Title);

        MainThread.BeginInvokeOnMainThread(() =>
        {
            Playlists.Add(new PlaylistViewModel { Id = playlist.Id, Title = playlist.Title });
            Title = string.Empty;
        });
    }

    [RelayCommand]
    private async Task DeletePlaylist(PlaylistViewModel playlist)
    {
        if (!await _playlistService.DeletePlaylistByIdAsync(playlist.Id))
            return;

        MainThread.BeginInvokeOnMainThread(() => Playlists.Remove(playlist));
        WeakReferenceMessenger.Default.Send(new MediaItemsRefreshMessage());
    }

    [RelayCommand]
    private async Task NavigateToDetailPage(int id)
    {
        await _navigator.GoToAsync($"{nameof(AppRoute.PlaylistDetails)}?{nameof(PlaylistViewModel.Id)}={id}");
    }

    [RelayCommand]
    private async Task NavigateToEditPlaylistPage(int playlistId)
    {
        await _navigator.GoToAsync($"{nameof(AppRoute.EditPlaylist)}?{nameof(PlaylistViewModel.Id)}={playlistId}");
    }

    public async Task GetPlaylistsAsync()
    {
        var playlists = await _playlistService.GetPlaylistsAsync();

        MainThread.BeginInvokeOnMainThread(() =>
        {
            Playlists.Clear();

            foreach (var playlistDto in playlists)
            {
                Playlists.Add(playlistDto.ToPlaylistViewModel());
            }
        });
    }


    private async Task UpdatePlaylistAsync(PlaylistViewModel playlist)
    {
        var playlistDto = playlist.ToPlaylistDto();
        if (!await _playlistService.UpdatePlaylistAsync(playlistDto))
            return;

        var playlistToUpdate = Playlists.FirstOrDefault(p => p.Id == playlist.Id);
        if (playlistToUpdate is null)
            return;

        MainThread.BeginInvokeOnMainThread(() => playlistToUpdate.Title = playlistDto.Title);
    }

    #region IRecipient<PlaylistUpdated>

    public async void Receive(PlaylistUpdatedMessage message)
    {
        await UpdatePlaylistAsync(message.Playlist);
    }

    #endregion
}