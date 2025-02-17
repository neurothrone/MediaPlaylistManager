using MediaPlaylistManager.DAL.EFCore.Shared.Entities;
using MediaPlaylistManager.DAL.EFCore.Shared.Interfaces;
using MediaPlaylistManager.WebApi.Data;
using Microsoft.EntityFrameworkCore;

namespace MediaPlaylistManager.WebApi.Repositories;

public class MediaItemRepository : IMediaItemRepository
{
    private readonly ApiDbContext _dbContext;

    public MediaItemRepository(ApiDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<int> CreateMediaItemAsync(MediaItemEntity mediaItem)
    {
        var entry = await _dbContext.MediaItems.AddAsync(mediaItem);
        await _dbContext.SaveChangesAsync();
        _dbContext.Entry(entry.Entity).State = EntityState.Detached;
        return entry.Entity.Id;
    }

    public async Task<IReadOnlyCollection<MediaItemEntity>> GetMediaItemsByPlaylistIdAsync(int playlistId)
    {
        return await _dbContext.MediaItems
            .AsNoTracking()
            .Where(m => m.PlaylistId == playlistId)
            .ToArrayAsync();
    }

    public async Task<MediaItemEntity?> GetMediaItemByIdAsync(int id)
    {
        return await _dbContext.MediaItems
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.Id == id);
    }

    public async Task<bool> UpdateMediaItemAsync(MediaItemEntity mediaItem)
    {
        var mediaItemToUpdate = await GetTrackedMediaItemByIdAsync(mediaItem.Id);
        if (mediaItemToUpdate is null)
            return false;

        mediaItemToUpdate.PlaylistId = mediaItem.PlaylistId;
        mediaItemToUpdate.FilePath = mediaItem.FilePath;
        mediaItemToUpdate.Title = mediaItem.Title;
        mediaItemToUpdate.Artist = mediaItem.Artist;
        mediaItemToUpdate.Duration = mediaItem.Duration;

        await _dbContext.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteMediaItemByIdAsync(int id)
    {
        var mediaItemToDelete = await GetTrackedMediaItemByIdAsync(id);
        if (mediaItemToDelete is null)
            return false;

        _dbContext.MediaItems.Remove(mediaItemToDelete);
        await _dbContext.SaveChangesAsync();
        return true;
    }

    public async Task<IReadOnlyCollection<MediaItemEntity>> SearchMediaItemsAsync(string query)
    {
        return await _dbContext.MediaItems
            .AsNoTracking()
            .Where(m =>
                m.Title.Contains(query) ||
                m.Artist.Contains(query)
            )
            .ToArrayAsync();
    }

    private async Task<MediaItemEntity?> GetTrackedMediaItemByIdAsync(int id)
    {
        return await _dbContext.MediaItems
            .FirstOrDefaultAsync(m => m.Id == id);
    }
}