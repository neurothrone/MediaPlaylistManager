using MediaPlaylistManager.DAL.EFCore.Shared.Entities;
using Microsoft.EntityFrameworkCore;

namespace MediaPlaylistManager.WebApi.Data;

public class DalDbContext : DbContext
{
    public DalDbContext(DbContextOptions<DalDbContext> options) : base(options)
    {
    }

    public DbSet<PlaylistEntity> Playlists { get; set; }
    public DbSet<MediaItemEntity> MediaItems { get; set; }
}