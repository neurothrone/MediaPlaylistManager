using CommunityToolkit.Mvvm.ComponentModel;

namespace MediaPlaylistManager.PL.Maui.Client.ViewModels;

public partial class PlaylistViewModel : ObservableObject
{
    public int Id { get; set; }

    [ObservableProperty]
    private string _title = string.Empty;
}