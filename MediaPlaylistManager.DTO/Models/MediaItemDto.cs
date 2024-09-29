namespace MediaPlaylistManager.DTO.Models;

public record MediaItemDto(
    int Id,
    int PlaylistId,
    string FilePath,
    string Title,
    string Artist,
    TimeSpan Duration
);