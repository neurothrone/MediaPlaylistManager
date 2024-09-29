using CommunityToolkit.Mvvm.ComponentModel;

namespace MediaPlaylistManager.PL.Maui.Client.ViewModels;

public partial class MediaItemViewModel : ObservableObject
{
    public int Id { get; set; }
    public int PlaylistId { get; set; }
    public string FilePath { get; set; } = string.Empty;

    [ObservableProperty]
    private string _title = string.Empty;

    [ObservableProperty]
    private string _artist = string.Empty;

    [ObservableProperty]
    private TimeSpan _duration;
}