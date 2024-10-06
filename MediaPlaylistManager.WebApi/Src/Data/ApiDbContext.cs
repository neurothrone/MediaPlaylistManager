using MediaPlaylistManager.DAL.EFCore.Shared.Entities;
using Microsoft.EntityFrameworkCore;

namespace MediaPlaylistManager.WebApi.Data;

public class ApiDbContext : DbContext
{
    public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options)
    {
    }

    public DbSet<PlaylistEntity> Playlists { get; set; }
    public DbSet<MediaItemEntity> MediaItems { get; set; }
}