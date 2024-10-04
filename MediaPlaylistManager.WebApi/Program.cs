using MediaPlaylistManager.DAL.EFCore.Shared.Entities;
using MediaPlaylistManager.DAL.EFCore.Shared.Interfaces;
using MediaPlaylistManager.WebApi.Data;
using MediaPlaylistManager.WebApi.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

#region Setup

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<DalDbContext>(options => options.UseSqlite(
    builder.Configuration.GetConnectionString(nameof(DalDbContext)) ??
    throw new InvalidOperationException($"Connection string '{nameof(DalDbContext)}' not found.")));

builder.Services.AddScoped<IPlaylistRepository, PlaylistRepository>();
builder.Services.AddScoped<IMediaItemRepository, MediaItemRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

#endregion

#region Playlist Routes

var playlistGroup = app.MapGroup("api/playlists")
    .WithTags("Playlists");

playlistGroup.MapPost("", async (PlaylistEntity playlist, IPlaylistRepository repository) =>
    {
        var createdPlaylist = await repository.CreatePlaylistAsync(playlist.Title);
        return Results.Created($"api/playlists/{createdPlaylist.Id}", createdPlaylist);
    })
    .WithName("Create Playlist")
    .WithOpenApi()
    .Produces<PlaylistEntity>(StatusCodes.Status201Created);

playlistGroup.MapGet("", async (IPlaylistRepository repository) =>
    {
        var playlists = await repository.GetPlaylistsAsync();
        return Results.Ok(playlists);
    })
    .WithName("Get All Playlists")
    .WithOpenApi();

playlistGroup.MapGet("/{id}", async (int id, IPlaylistRepository repository) =>
        await repository.GetPlaylistByIdAsync(id)
            is { } playlist
            ? Results.Ok(playlist)
            : Results.NotFound()
    )
    .WithName("Get Playlist by Id")
    .WithOpenApi();

// playlistGroup.MapPut("/{id}", async (int id, PlaylistEntity playlist, IPlaylistRepository repository) =>
playlistGroup.MapPut("", async (PlaylistEntity playlist, IPlaylistRepository repository) =>
    {
        var updated = await repository.UpdatePlaylistAsync(playlist);
        return updated ? Results.NoContent() : Results.NotFound();
    })
    .WithName("Update Playlist by Id")
    .WithOpenApi();

playlistGroup.MapDelete("/{id}", async (int id, IPlaylistRepository repository) =>
    {
        var deleted = await repository.DeletePlaylistByIdAsync(id);
        return deleted ? Results.NoContent() : Results.NotFound();
    })
    .WithName("Delete Playlist by Id")
    .WithOpenApi();

#endregion

#region MediaItem Routes

var mediaItemsGroup = app.MapGroup("api/media-items")
    .WithTags("MediaItems");

mediaItemsGroup.MapPost("", async (MediaItemEntity mediaItem, IMediaItemRepository repository) =>
    {
        var newMediaItem = new MediaItemEntity
        {
            PlaylistId = mediaItem.PlaylistId,
            FilePath = mediaItem.FilePath,
            Title = mediaItem.Title,
            Artist = mediaItem.Artist,
            Duration = mediaItem.Duration
        };
        var createdItemId = await repository.CreateMediaItemAsync(newMediaItem);
        return Results.Created($"api/media-items/{createdItemId}", new { Id = createdItemId });
    })
    .WithName("Create MediaItem")
    .WithOpenApi()
    .Produces<int>(StatusCodes.Status201Created);

mediaItemsGroup.MapGet("/playlist/{id}", async (int id, IMediaItemRepository repository) =>
        await repository.GetMediaItemsByPlaylistIdAsync(id)
            is { } mediaItems
            ? Results.Ok(mediaItems)
            : Results.NotFound()
    )
    .WithName("Get MediaItem:s by Playlist Id")
    .WithOpenApi();

mediaItemsGroup.MapGet("/{id}", async (int id, IMediaItemRepository repository) =>
        await repository.GetMediaItemByIdAsync(id)
            is { } mediaItem
            ? Results.Ok(mediaItem)
            : Results.NotFound()
    )
    .WithName("Get MediaItem by Id")
    .WithOpenApi();

mediaItemsGroup.MapPut("", async (MediaItemEntity mediaItem, IMediaItemRepository repository) =>
    {
        var updated = await repository.UpdateMediaItemAsync(mediaItem);
        return updated ? Results.NoContent() : Results.NotFound();
    })
    .WithName("Update MediaItem by Id")
    .WithOpenApi();

mediaItemsGroup.MapDelete("/{id}", async (int id, IMediaItemRepository repository) =>
    {
        var deleted = await repository.DeleteMediaItemByIdAsync(id);
        return deleted ? Results.NoContent() : Results.NotFound();
    })
    .WithName("Delete MediaItem by Id")
    .WithOpenApi();

mediaItemsGroup.MapGet("", async ([FromQuery] string? query, IMediaItemRepository repository) =>
    {
        List<MediaItemEntity> mediaItems = [];
        if (string.IsNullOrEmpty(query))
            return Results.Ok(mediaItems);

        mediaItems = await repository.SearchMediaItemsAsync(query);
        return Results.Ok(mediaItems);
    })
    .WithName("Search All Contact:s")
    .WithOpenApi();

#endregion

app.Run();