namespace MediaPlaylistManager.SL.Interfaces;

public interface INavigator
{
    Task GoToAsync(string route);
    Task GoBackAsync();
}