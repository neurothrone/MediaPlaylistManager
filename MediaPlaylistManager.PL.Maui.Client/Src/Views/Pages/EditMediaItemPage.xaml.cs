using MediaPlaylistManager.PL.Maui.Client.ViewModels;

namespace MediaPlaylistManager.PL.Maui.Client.Views.Pages;

public partial class EditMediaItemPage : ContentPage
{
    public EditMediaItemPage(EditMediaItemViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}