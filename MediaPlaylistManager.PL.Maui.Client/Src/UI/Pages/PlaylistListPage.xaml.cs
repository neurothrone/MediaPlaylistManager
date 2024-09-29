using MediaPlaylistManager.PL.Maui.Client.ViewModels;

namespace MediaPlaylistManager.PL.Maui.Client.UI.Pages;

public partial class PlaylistListPage : ContentPage
{
    public PlaylistListPage(PlaylistListViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is PlaylistListViewModel viewModel)
            await viewModel.GetPlaylistsAsync();
    }
}