using System.IO;
using Mono.Data.Sqlite;
using UnityEngine;

public static class DatabaseManager
{
    private const string DatabaseName = "game.db";
    private static string DbPath => Path.Combine(Application.persistentDataPath, DatabaseName);
    private static SqliteConnection _connection;

    public static SqliteConnection Connection => _connection;

    public static void Initialize()
    {
        bool createTables = !File.Exists(DbPath);
        _connection = new SqliteConnection($"URI=file:{DbPath}");
        _connection.Open();
        if (createTables)
        {
            CreateTables();
        }
    }

    private static void CreateTables()
    {
        using (var cmd = _connection.CreateCommand())
        {
            cmd.CommandText = @"
                PRAGMA foreign_keys = ON;
                CREATE TABLE IF NOT EXISTS Accounts(
                    id INTEGER PRIMARY KEY AUTOINCREMENT,
                    username TEXT UNIQUE NOT NULL,
                    password TEXT NOT NULL
                );
                CREATE TABLE IF NOT EXISTS Levels(
                    id INTEGER PRIMARY KEY,
                    name TEXT
                );
                CREATE TABLE IF NOT EXISTS AccountLevels(
                    id INTEGER PRIMARY KEY AUTOINCREMENT,
                    account_id INTEGER NOT NULL,
                    level_id INTEGER NOT NULL,
                    FOREIGN KEY(account_id) REFERENCES Accounts(id),
                    FOREIGN KEY(level_id) REFERENCES Levels(id)
                );
                CREATE TABLE IF NOT EXISTS Skins(
                    id INTEGER PRIMARY KEY,
                    key TEXT
                );
                CREATE TABLE IF NOT EXISTS AccountSkins(
                    id INTEGER PRIMARY KEY AUTOINCREMENT,
                    account_id INTEGER NOT NULL,
                    skin_id INTEGER NOT NULL,
                    FOREIGN KEY(account_id) REFERENCES Accounts(id),
                    FOREIGN KEY(skin_id) REFERENCES Skins(id)
                );";
            cmd.ExecuteNonQuery();
        }
    }

    public static void Close()
    {
        _connection?.Close();
        _connection = null;
    }
}
