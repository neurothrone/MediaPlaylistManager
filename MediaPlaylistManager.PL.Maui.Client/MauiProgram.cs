using CommunityToolkit.Maui;
using MediaPlaylistManager.BLL.Interfaces;
using MediaPlaylistManager.BLL.Managers;
using MediaPlaylistManager.DAL.Data;
using MediaPlaylistManager.DAL.Interfaces;
using MediaPlaylistManager.DAL.Repositories;
using MediaPlaylistManager.PL.Maui.Client.Enums;
using MediaPlaylistManager.PL.Maui.Client.Services;
using MediaPlaylistManager.PL.Maui.Client.UI.Controls;
using MediaPlaylistManager.PL.Maui.Client.UI.Pages;
using MediaPlaylistManager.PL.Maui.Client.ViewModels;
using MediaPlaylistManager.SL.Interfaces;
using MediaPlaylistManager.SL.Services;
using MediaPlaylistManager.UtilitiesLib;
using Microsoft.Extensions.Logging;

namespace MediaPlaylistManager.PL.Maui.Client;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit() // Initialize the .NET MAUI Community Toolkit
            .UseMauiCommunityToolkitMediaElement() // Initialize the .NET MAUI Community Toolkit MediaElement
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                fonts.AddFont("Font Awesome 6 Free-Solid-900.otf", "FaSolid");
            })
            .RegisterServices()
            .RegisterViewModels()
            .RegisterViews();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        RegisterRoutes();

        var app = builder.Build();

        // NOTE: Initialize ServiceHelper for use cases when constructor injection is not
        //       possible, as is the case for [MediaItemPlayer.xaml.cs].
        ServiceHelper.Initialize(app.Services);
        return app;
    }

    private static MauiAppBuilder RegisterServices(this MauiAppBuilder builder)
    {
        builder.Services.AddSingleton<DataStore>();

        builder.Services.AddSingleton<IPlaylistService, PlaylistService>();
        builder.Services.AddSingleton<IPlaylistManager, PlaylistManager>();
        builder.Services.AddSingleton<IPlaylistRepository, PlaylistRepository>();

        builder.Services.AddSingleton<IMediaItemService, MediaItemService>();
        builder.Services.AddSingleton<IMediaItemManager, MediaItemManager>();
        builder.Services.AddSingleton<IMediaItemRepository, MediaItemRepository>();

        builder.Services.AddSingleton<INavigator, Navigator>();
        builder.Services.AddSingleton<IFileService, FileService>();
        builder.Services.AddSingleton<IMediaMetadataService, MediaMetadataService>();

        return builder;
    }

    private static MauiAppBuilder RegisterViewModels(this MauiAppBuilder builder)
    {
        builder.Services.AddSingleton<MediaItemPlayerViewModel>();
        builder.Services.AddSingleton<PlayerDetailsViewModel>();
        builder.Services.AddSingleton<PlaylistListViewModel>();
        builder.Services.AddSingleton<SearchViewModel>();

        builder.Services.AddTransient<PlaylistDetailViewModel>();
        builder.Services.AddTransient<EditPlaylistViewModel>();
        builder.Services.AddTransient<AddMediaItemViewModel>();
        builder.Services.AddTransient<EditMediaItemViewModel>();

        return builder;
    }

    private static void RegisterViews(this MauiAppBuilder builder)
    {
        // !: Pages
        builder.Services.AddSingleton<PlayerPage>();
        builder.Services.AddSingleton<PlaylistListPage>();
        builder.Services.AddSingleton<SearchPage>();

        builder.Services.AddTransient<PlaylistDetailPage>();
        builder.Services.AddTransient<EditPlaylistPage>();
        builder.Services.AddTransient<AddMediaItemPage>();
        builder.Services.AddTransient<EditMediaItemPage>();

        // !: Controls
        builder.Services.AddSingleton<MediaItemPlayer>();
    }

    private static void RegisterRoutes()
    {
        Routing.RegisterRoute(nameof(AppRoute.PlaylistDetails), typeof(PlaylistDetailPage));
        Routing.RegisterRoute(nameof(AppRoute.EditPlaylist), typeof(EditPlaylistPage));
        Routing.RegisterRoute(nameof(AppRoute.AddMediaItem), typeof(AddMediaItemPage));
        Routing.RegisterRoute(nameof(AppRoute.EditMediaItem), typeof(EditMediaItemPage));
    }
}