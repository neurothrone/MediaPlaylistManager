using CommunityToolkit.Maui;
using MediaPlaylistManager.BLL.Interfaces;
using MediaPlaylistManager.BLL.Managers;
using MediaPlaylistManager.DAL.EFCore.Shared.Interfaces;
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

        RegisterRoutes();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        var app = builder.Build();

        // NOTE: Initialize ServiceHelper for use cases when constructor injection is not
        //       possible, as is the case for [MediaItemPlayer.xaml.cs].
        ServiceHelper.Initialize(app.Services);

        return app;
    }

    private static MauiAppBuilder RegisterServices(this MauiAppBuilder builder)
    {
        // !: DAL -> SQLite data source (WebApi)
        builder.Services.AddSingleton<IPlaylistRepository, DAL.EFCore.WebApi.Repositories.PlaylistWebApiRepository>();
        builder.Services.AddSingleton<IMediaItemRepository, DAL.EFCore.WebApi.Repositories.MediaItemWebApiRepository>();

        // !: DAL -> SQLite data source (sqlite-net-pcl)
        // builder.Services.AddSingleton<DAL.Local.Sqlite.Data.MediaDatabase>();
        // builder.Services.AddSingleton<IPlaylistRepository, DAL.Local.Sqlite.Data.MediaDatabase>();
        // builder.Services.AddSingleton<IMediaItemRepository, DAL.Local.Sqlite.Data.MediaDatabase>();

        // !: DAL -> In-Memory data source
        // builder.Services.AddSingleton<DAL.Local.InMemory.Data.DataStore>();
        // builder.Services.AddSingleton<IPlaylistRepository, DAL.Local.InMemory.Repositories.PlaylistRepository>();
        // builder.Services.AddSingleton<IMediaItemRepository, DAL.Local.InMemory.Repositories.MediaItemRepository>();

        // !: BLL
        builder.Services.AddSingleton<IPlaylistManager, PlaylistManager>();
        builder.Services.AddSingleton<IMediaItemManager, MediaItemManager>();

        // !: SL
        builder.Services.AddSingleton<IPlaylistService, PlaylistService>();
        builder.Services.AddSingleton<IMediaItemService, MediaItemService>();
        builder.Services.AddSingleton<IMediaMetadataService, MediaMetadataService>();

        builder.Services.AddSingleton<IDialogService, DialogService>();
        builder.Services.AddSingleton<IFileService, FileService>();
        builder.Services.AddSingleton<INavigator, Navigator>();

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