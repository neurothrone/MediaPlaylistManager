using MediaPlaylistManager.SL.Interfaces;

namespace MediaPlaylistManager.PL.Maui.Client.Services;

public class Navigator : INavigator
{
    public async Task GoToAsync(string route)
    {
        await Shell.Current.GoToAsync(route);
    }

    public async Task GoBackAsync()
    {
        await Shell.Current.GoToAsync("..");
    }
}