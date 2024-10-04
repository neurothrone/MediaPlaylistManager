using MediaPlaylistManager.PL.Maui.Client.ViewModels;

namespace MediaPlaylistManager.PL.Maui.Client.Views.Pages;

public partial class EditPlaylistPage : ContentPage
{
    public EditPlaylistPage(EditPlaylistViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}