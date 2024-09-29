using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using MediaPlaylistManager.PL.Maui.Client.Messages;

namespace MediaPlaylistManager.PL.Maui.Client.ViewModels;

public partial class PlayerDetailsViewModel :
    ObservableObject,
    IRecipient<MediaItemLoadedMessage>
{
    [ObservableProperty]
    private MediaItemViewModel? _mediaItem;

    public PlayerDetailsViewModel()
    {
        WeakReferenceMessenger.Default.Register(this);
    }

    ~PlayerDetailsViewModel() => WeakReferenceMessenger.Default.Unregister<MediaItemLoadedMessage>(this);

    private void LoadItemDetails(MediaItemViewModel mediaItem)
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            MediaItem = mediaItem;
        });
    }

    #region IRecipient<MediaItemLoadedMessage>

    public void Receive(MediaItemLoadedMessage message)
    {
        LoadItemDetails(message.MediaItem);
    }

    #endregion
}