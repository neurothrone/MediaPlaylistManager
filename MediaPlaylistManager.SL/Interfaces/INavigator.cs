namespace MediaPlaylistManager.SL.Interfaces;

public interface INavigator
{
    Task GoToAsync(string route);
    Task GoToAsync(string route, IDictionary<string, object> parameters);
    Task GoBackAsync();
}