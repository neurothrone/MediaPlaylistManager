using MediaPlaylistManager.PL.Maui.Client.ViewModels;
using MediaPlaylistManager.UtilitiesLib;

namespace MediaPlaylistManager.PL.Maui.Client.Views.Controls;

public partial class PlayerDetails : ContentView
{
    public PlayerDetails()
    {
        InitializeComponent();

        var viewModel = ServiceHelper.GetRequiredService<PlayerDetailsViewModel>();
        BindingContext = viewModel;
    }
}