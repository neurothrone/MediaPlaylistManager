using MediaPlaylistManager.DAL.Local.Shared.Interfaces;
using MediaPlaylistManager.DAL.Local.Shared.Entities;
using SQLite;

namespace MediaPlaylistManager.DAL.Local.Sqlite.Data;

// Source:
// https://learn.microsoft.com/en-us/dotnet/maui/data-cloud/database-sqlite?view=net-maui-8.0

public class MediaDatabase :
    IPlaylistRepository,
    IMediaItemRepository
{
    private SQLiteAsyncConnection _database = null!;

    private async Task Init()
    {
        // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
        if (_database is not null)
            return;

        _database = new SQLiteAsyncConnection(
            DatabaseConstants.DatabasePath,
            DatabaseConstants.Flags
        );

#if DEBUG
        await _database.CreateTableAsync<PlaylistEntity>()
            .ContinueWith(results => Console.WriteLine($"Playlist Table Result: {results.Result}"));
        await _database.CreateTableAsync<MediaItemEntity>()
            .ContinueWith(results => Console.WriteLine($"MediaItem Table Result: {results.Result}"));
#else
        await _database.CreateTableAsync<PlaylistEntity>();
        await _database.CreateTableAsync<MediaItemEntity>();
#endif
    }

    #region Playlists

    public async Task<PlaylistEntity> CreatePlaylistAsync(string title)
    {
        await Init();
        var playlist = new PlaylistEntity { Title = title };
        await _database.InsertAsync(playlist);
        return playlist;
    }

    public async Task<List<PlaylistEntity>> GetPlaylistsAsync()
    {
        await Init();
        return await _database
            .Table<PlaylistEntity>()
            .ToListAsync();
    }

    public async Task<PlaylistEntity?> GetPlaylistByIdAsync(int id)
    {
        await Init();
        return await _database
            .Table<PlaylistEntity>()
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<bool> UpdatePlaylistAsync(PlaylistEntity playlist)
    {
        await Init();
        return await _database.UpdateAsync(playlist) > 0;
    }

    public async Task<bool> DeletePlaylistByIdAsync(int id)
    {
        var playlistToDelete = await GetPlaylistByIdAsync(id);
        if (playlistToDelete is null)
            return false;

        var mediaItems = await GetMediaItemsByPlaylistIdAsync(id);

        // Using a transaction here ensures all deletions happen or none do.
        // Source:
        // https://github.com/praeclarum/sqlite-net/wiki/Transactions
        var result = false;
        await _database.RunInTransactionAsync(transaction =>
        {
            foreach (var mediaItemEntity in mediaItems)
            {
                transaction.Delete(mediaItemEntity);
            }

            var playlistRowsAffected = transaction.Delete(playlistToDelete);
            result = playlistRowsAffected > 0;
        });

        return result;
    }

    #endregion

    #region MediaItems

    public async Task<int> CreateMediaItemAsync(MediaItemEntity mediaItem)
    {
        await Init();
        await _database.InsertAsync(mediaItem);
        return mediaItem.Id;
    }

    public async Task<List<MediaItemEntity>> GetMediaItemsByPlaylistIdAsync(int playlistId)
    {
        await Init();
        return await _database
            .Table<MediaItemEntity>()
            .Where(m => m.PlaylistId == playlistId)
            .ToListAsync();
    }

    public async Task<MediaItemEntity?> GetMediaItemByIdAsync(int id)
    {
        await Init();
        return await _database
            .Table<MediaItemEntity>()
            .FirstOrDefaultAsync(m => m.Id == id);
    }

    public async Task<bool> UpdateMediaItemAsync(MediaItemEntity mediaItem)
    {
        await Init();
        return await _database.UpdateAsync(mediaItem) > 0;
    }

    public async Task<bool> DeleteMediaItemByIdAsync(int id)
    {
        await Init();
        return await _database.DeleteAsync<MediaItemEntity>(id) > 0;
    }

    public async Task<List<MediaItemEntity>> SearchMediaItemsAsync(string query)
    {
        await Init();
        return await _database
            .Table<MediaItemEntity>()
            .Where(m =>
                m.Title.Contains(query) ||
                m.Artist.Contains(query)
            )
            .ToListAsync();
    }

    #endregion
}