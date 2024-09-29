namespace MediaPlaylistManager.PL.Maui.Client;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
        
        if (Current != null)
            Current.UserAppTheme = AppTheme.Light;

        MainPage = new AppShell();
    }
}