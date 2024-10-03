using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using MediaPlaylistManager.PL.Maui.Client.Enums;
using MediaPlaylistManager.PL.Maui.Client.Messages;

namespace MediaPlaylistManager.PL.Maui.Client.ViewModels;

public partial class MediaItemPlayerViewModel :
    ObservableObject,
    IRecipient<MediaItemLoadedMessage>
{
    [ObservableProperty]
    [NotifyCanExecuteChangedFor(
        nameof(PlayCommand),
        nameof(PauseCommand),
        nameof(StopCommand))]
    private MediaSource? _source;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(
        nameof(PlayCommand),
        nameof(PauseCommand),
        nameof(StopCommand))]
    private PlaybackState _playbackState = PlaybackState.Stopped;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(SetVolumeToMinCommand))]
    [NotifyCanExecuteChangedFor(nameof(SetVolumeToMaxCommand))]
    private double _volume = 0.5;

    public event Action? OnPlayRequested;
    public event Action? OnPauseRequested;
    public event Action? OnStopRequested;

    public MediaItemPlayerViewModel()
    {
        WeakReferenceMessenger.Default.Register(this);
    }

    ~MediaItemPlayerViewModel() => WeakReferenceMessenger.Default.Unregister<MediaItemLoadedMessage>(this);

    private bool HasMediaSource() => Source is not null;
    private bool CanPlay() => HasMediaSource() && PlaybackState is not PlaybackState.Playing;
    private bool CanPause() => HasMediaSource() && PlaybackState is PlaybackState.Playing;
    private bool CanStop() => HasMediaSource() && PlaybackState is PlaybackState.Playing;

    private bool CanSetVolumeToMin() => !Volume.Equals(0d);
    private bool CanSetVolumeToMax() => !Volume.Equals(1d);

    [RelayCommand(CanExecute = nameof(CanPlay))]
    private void Play()
    {
        OnPlayRequested?.Invoke();
        MainThread.BeginInvokeOnMainThread(() => PlaybackState = PlaybackState.Playing);
    }

    [RelayCommand(CanExecute = nameof(CanPause))]
    private void Pause()
    {
        OnPauseRequested?.Invoke();
        MainThread.BeginInvokeOnMainThread(() => PlaybackState = PlaybackState.Paused);
    }

    [RelayCommand(CanExecute = nameof(CanStop))]
    private void Stop()
    {
        OnStopRequested?.Invoke();
        MainThread.BeginInvokeOnMainThread(() => PlaybackState = PlaybackState.Stopped);
    }

    [RelayCommand(CanExecute = nameof(CanSetVolumeToMin))]
    private void SetVolumeToMin() => MainThread.BeginInvokeOnMainThread(() => Volume = 0d);

    [RelayCommand(CanExecute = nameof(CanSetVolumeToMax))]
    private void SetVolumeToMax() => MainThread.BeginInvokeOnMainThread(() => Volume = 1d);

    private void LoadMediaItem(string filePath)
    {
        Stop();
        
        MainThread.BeginInvokeOnMainThread(() => Source = MediaSource.FromFile(filePath));
    }

    #region IRecipient<MediaItemLoadedMessage>

    public void Receive(MediaItemLoadedMessage message)
    {
        LoadMediaItem(message.MediaItem.FilePath);
    }

    #endregion
}