using MediaPlaylistManager.DAL.EFCore.Sqlite.Entities;
using Microsoft.EntityFrameworkCore;

namespace MediaPlaylistManager.DAL.EFCore.Sqlite.Data;

public class DalDbContext : DbContext
{
    public DalDbContext(DbContextOptions<DalDbContext> options) : base(options)
    {
    }

    public DbSet<PlaylistEntity> Playlists { get; set; }
    public DbSet<MediaItemEntity> MediaItems { get; set; }

    // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    // {
    //     base.OnConfiguring(optionsBuilder);
    //
    //     var path = Path.Combine(FileSystem.AppDataDirectory, "MediaPlaylistManager.db3");
    //     optionsBuilder.UseSqlite($"Data Source={path}");
    // }
}