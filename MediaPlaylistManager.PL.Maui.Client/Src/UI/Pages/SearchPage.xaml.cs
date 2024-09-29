using MediaPlaylistManager.PL.Maui.Client.ViewModels;

namespace MediaPlaylistManager.PL.Maui.Client.UI.Pages;

public partial class SearchPage : ContentPage
{
    public SearchPage(SearchViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}