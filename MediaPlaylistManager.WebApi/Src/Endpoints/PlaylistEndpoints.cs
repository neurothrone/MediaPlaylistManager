using MediaPlaylistManager.DAL.EFCore.Shared.Entities;
using MediaPlaylistManager.DAL.EFCore.Shared.Interfaces;

namespace MediaPlaylistManager.WebApi.Endpoints;

public static class PlaylistEndpoints
{
    public static void MapPlaylistEndpoints(this IEndpointRouteBuilder routes)
    {
        var playlistGroup = routes.MapGroup("api/playlists")
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
    }
}