using MediaPlaylistManager.PL.Maui.Client.ViewModels;

namespace MediaPlaylistManager.PL.Maui.Client.UI.Pages;

public partial class EditMediaItemPage : ContentPage
{
    public EditMediaItemPage(EditMediaItemViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}