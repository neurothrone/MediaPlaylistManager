using MediaPlaylistManager.DAL.EFCore.Sqlite.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace MediaPlaylistManager.DAL.EFCore.Sqlite.Migrations;

// Source:
// https://learn.microsoft.com/en-us/ef/core/cli/dbcontext-creation

public class MigrationContextFactory : IDesignTimeDbContextFactory<DalDbContext>
{
    public DalDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<DalDbContext>();

        var dbPath = "MediaPlaylistManager.db3";
        optionsBuilder.UseSqlite($"Data Source={dbPath}");

        return new DalDbContext(optionsBuilder.Options);
    }
}