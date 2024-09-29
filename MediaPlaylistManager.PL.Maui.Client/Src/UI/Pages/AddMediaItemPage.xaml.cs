using MediaPlaylistManager.PL.Maui.Client.ViewModels;

namespace MediaPlaylistManager.PL.Maui.Client.UI.Pages;

public partial class AddMediaItemPage : ContentPage
{
    public AddMediaItemPage(AddMediaItemViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}