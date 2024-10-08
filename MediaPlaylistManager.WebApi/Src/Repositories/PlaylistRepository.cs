using MediaPlaylistManager.DAL.EFCore.Shared.Entities;
using MediaPlaylistManager.DAL.EFCore.Shared.Interfaces;
using MediaPlaylistManager.WebApi.Data;
using Microsoft.EntityFrameworkCore;

namespace MediaPlaylistManager.WebApi.Repositories;

public class PlaylistRepository : IPlaylistRepository
{
    private readonly ApiDbContext _dbContext;

    public PlaylistRepository(ApiDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<PlaylistEntity> CreatePlaylistAsync(string title)
    {
        var entry = await _dbContext.Playlists.AddAsync(
            new PlaylistEntity { Title = title });
        await _dbContext.SaveChangesAsync();
        _dbContext.Entry(entry.Entity).State = EntityState.Detached;
        return entry.Entity;
    }

    public async Task<IReadOnlyCollection<PlaylistEntity>> GetPlaylistsAsync()
    {
        return await _dbContext.Playlists
            .AsNoTracking()
            .ToArrayAsync();
    }

    public async Task<PlaylistEntity?> GetPlaylistByIdAsync(int id)
    {
        return await _dbContext.Playlists
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == id);
    }
    
    public async Task<PlaylistEntity?> GetPlaylistWithMediaItemsByIdAsync(int id)
    {
        return await _dbContext.Playlists
            .AsNoTracking()
            .Include(m => m.MediaItems)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<bool> UpdatePlaylistAsync(PlaylistEntity playlist)
    {
        var playlistToUpdate = await GetTrackedPlaylistByIdAsync(playlist.Id);
        if (playlistToUpdate is null)
            return false;

        playlistToUpdate.Title = playlist.Title;
        await _dbContext.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeletePlaylistByIdAsync(int id)
    {
        var playlistToDelete = await GetTrackedPlaylistByIdAsync(id);
        if (playlistToDelete is null)
            return false;

        // Using a transaction here ensures all deletions happen or none do.
        // Source:
        // https://learn.microsoft.com/en-us/ef/core/saving/transactions
        await using var transaction = await _dbContext.Database.BeginTransactionAsync();
        try
        {
            _dbContext.MediaItems.RemoveRange(playlistToDelete.MediaItems);
            await _dbContext.SaveChangesAsync();

            _dbContext.Playlists.Remove(playlistToDelete);
            await _dbContext.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            return false;
        }

        return true;
    }

    private async Task<PlaylistEntity?> GetTrackedPlaylistByIdAsync(int id)
    {
        return await _dbContext.Playlists
            .FirstOrDefaultAsync(p => p.Id == id);
    }
}