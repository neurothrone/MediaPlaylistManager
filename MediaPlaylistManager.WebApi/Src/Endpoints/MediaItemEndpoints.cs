using MediaPlaylistManager.DAL.EFCore.Shared.Entities;
using MediaPlaylistManager.DAL.EFCore.Shared.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MediaPlaylistManager.WebApi.Endpoints;

public static class MediaItemEndpoints
{
    public static void MapMediaItemEndpoints(this IEndpointRouteBuilder routes)
    {
        var mediaItemsGroup = routes.MapGroup("api/media-items")
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
    }
}