﻿namespace MediaPlaylistManager.PL.Maui.Client;

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
        const double windowHeight = 800d;
        const double windowWidth = 700d;

        window.MinimumHeight = windowHeight;
        window.MinimumWidth = windowWidth;
        
        window.Height = windowHeight;
        window.Width = windowWidth;
#endif

        return window;
    }
}