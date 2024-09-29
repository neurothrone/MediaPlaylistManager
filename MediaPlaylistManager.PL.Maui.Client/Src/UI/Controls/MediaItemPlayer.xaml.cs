using CommunityToolkit.Maui.Core.Primitives;
using MediaPlaylistManager.PL.Maui.Client.ViewModels;
using MediaPlaylistManager.UtilitiesLib;

namespace MediaPlaylistManager.PL.Maui.Client.UI.Controls;

public partial class MediaItemPlayer : 
    ContentView, 
    IDisposable
{
    public MediaItemPlayer()
    {
        InitializeComponent();

        var viewModel = ServiceHelper.GetRequiredService<MediaItemPlayerViewModel>();
        viewModel.OnPlayRequested += MediaPlayer_OnPlayRequested;
        viewModel.OnPauseRequested += MediaPlayer_OnPauseRequested;
        viewModel.OnStopRequested += MediaPlayer_OnStopRequested;

        BindingContext = viewModel;
    }

    private void MediaPlayer_OnPlayRequested()
    {
        if (Player.Source is null)
            return;

        if (Player.CurrentState != MediaElementState.Paused)
            Player.Stop();

        Player.Play();
    }

    private void MediaPlayer_OnPauseRequested()
    {
        if (Player.Source is null)
            return;

        Player.Pause();
    }

    private void MediaPlayer_OnStopRequested()
    {
        if (Player.Source is null)
            return;

        Player.Stop();
    }

    #region IDisposable

    public void Dispose()
    {
        Player?.Dispose();

        if (BindingContext is not MediaItemPlayerViewModel viewModel)
            return;

        viewModel.OnPlayRequested -= MediaPlayer_OnPlayRequested;
        viewModel.OnPauseRequested -= MediaPlayer_OnPauseRequested;
        viewModel.OnStopRequested -= MediaPlayer_OnStopRequested;
    }

    #endregion
}