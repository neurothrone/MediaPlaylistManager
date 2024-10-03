using Microsoft.Maui.Storage;

namespace MediaPlaylistManager.DAL.Sqlite.Data;

// Source:
// https://learn.microsoft.com/en-us/dotnet/maui/data-cloud/database-sqlite?view=net-maui-8.0

public static class DatabaseConstants
{
    private const string DatabaseFilename = "MediaPlaylistManagerDb.db3";
    public static string DatabasePath => Path.Combine(FileSystem.AppDataDirectory, DatabaseFilename);

    public const SQLite.SQLiteOpenFlags Flags =
        // open the database in read/write mode
        SQLite.SQLiteOpenFlags.ReadWrite |
        // create the database if it doesn't exist
        SQLite.SQLiteOpenFlags.Create |
        // enable multi-threaded database access
        SQLite.SQLiteOpenFlags.SharedCache;
}