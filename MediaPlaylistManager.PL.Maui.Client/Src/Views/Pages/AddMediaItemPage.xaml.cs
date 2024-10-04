using MediaPlaylistManager.PL.Maui.Client.ViewModels;

namespace MediaPlaylistManager.PL.Maui.Client.Views.Pages;

public partial class AddMediaItemPage : ContentPage
{
    public AddMediaItemPage(AddMediaItemViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}