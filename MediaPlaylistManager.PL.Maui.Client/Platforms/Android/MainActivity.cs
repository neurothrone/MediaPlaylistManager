using Android.App;
using Android.Content.PM;
using Android.OS;

namespace MediaPlaylistManager.PL.Maui.Client;

[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true,
    // LaunchMode = LaunchMode.SingleTop,
    // !: MediaElement setup start
    LaunchMode = LaunchMode.SingleTask,
    ResizeableActivity = true,
    // !: MediaElement setup end
    ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode |
                           ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity
{
}