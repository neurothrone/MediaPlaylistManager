namespace MediaPlaylistManager.SL.Interfaces;

public interface IDialogService
{
    Task ShowAlertAsync(string title, string message, string accept);
}