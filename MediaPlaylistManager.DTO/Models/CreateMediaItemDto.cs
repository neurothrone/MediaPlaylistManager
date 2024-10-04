namespace MediaPlaylistManager.DTO.Models;

public record CreateMediaItemDto(
    int PlaylistId,
    string FilePath,
    string Title,
    string Artist,
    TimeSpan Duration
);