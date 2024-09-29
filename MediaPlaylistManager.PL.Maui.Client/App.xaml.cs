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

    // Source:
    // https://learn.microsoft.com/en-us/dotnet/maui/fundamentals/windows?view=net-maui-8.0

    protected override Window CreateWindow(IActivationState? activationState)
    {
        var window = base.CreateWindow(activationState);

#if WINDOWS || MACCATALYST
        window.MinimumHeight = 800d;
        window.MinimumWidth = 700d;
#endif

        return window;
    }
}