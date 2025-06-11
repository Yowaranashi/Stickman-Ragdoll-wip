using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;
using System.IO;

public class DatabaseManager
{
    private const string DatabaseName = "GameData.db";
    private const int PlayerId = 1;

    private string ConnectionString => "URI=file:" + Path.Combine(Application.persistentDataPath, DatabaseName);

    public DatabaseManager()
    {
        InitializeDatabase();
    }

    private void InitializeDatabase()
    {
        using (var connection = new SqliteConnection(ConnectionString))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText =
                    "CREATE TABLE IF NOT EXISTS Players(" +
                    "id INTEGER PRIMARY KEY AUTOINCREMENT," +
                    "name TEXT NOT NULL);";
                command.ExecuteNonQuery();

                command.CommandText = "INSERT OR IGNORE INTO Players(id,name) VALUES (1,'Default');";
                command.ExecuteNonQuery();

                command.CommandText =
                    "CREATE TABLE IF NOT EXISTS Skins(" +
                    "id INTEGER PRIMARY KEY AUTOINCREMENT," +
                    "key TEXT UNIQUE NOT NULL);";
                command.ExecuteNonQuery();

                command.CommandText =
                    "CREATE TABLE IF NOT EXISTS PlayerProgress(" +
                    "player_id INTEGER PRIMARY KEY," +
                    "money INTEGER NOT NULL," +
                    "level INTEGER NOT NULL," +
                    "FOREIGN KEY(player_id) REFERENCES Players(id) ON DELETE CASCADE);";
                command.ExecuteNonQuery();

                command.CommandText =
                    "INSERT OR IGNORE INTO PlayerProgress(player_id,money,level) VALUES (1,0,1);";
                command.ExecuteNonQuery();

                command.CommandText =
                    "CREATE TABLE IF NOT EXISTS PlayerSkins(" +
                    "player_id INTEGER NOT NULL," +
                    "skin_id INTEGER NOT NULL," +
                    "opened INTEGER NOT NULL," +
                    "chosen INTEGER NOT NULL," +
                    "PRIMARY KEY(player_id, skin_id)," +
                    "FOREIGN KEY(player_id) REFERENCES Players(id) ON DELETE CASCADE," +
                    "FOREIGN KEY(skin_id) REFERENCES Skins(id) ON DELETE CASCADE);";
                command.ExecuteNonQuery();
            }
        }
    }

    private int GetOrCreateSkinId(SqliteConnection connection, string key)
    {
        using (var command = connection.CreateCommand())
        {
            command.CommandText = "SELECT id FROM Skins WHERE key=@key";
            command.Parameters.AddWithValue("@key", key);
            object result = command.ExecuteScalar();
            if (result != null && result != DBNull.Value)
            {
                return System.Convert.ToInt32(result);
            }

            command.CommandText = "INSERT INTO Skins(key) VALUES(@key)";
            command.Parameters.Clear();
            command.Parameters.AddWithValue("@key", key);
            command.ExecuteNonQuery();

            command.CommandText = "SELECT id FROM Skins WHERE key=@key";
            command.Parameters.Clear();
            command.Parameters.AddWithValue("@key", key);
            result = command.ExecuteScalar();
            return System.Convert.ToInt32(result);
        }
    }

    public (bool opened, bool chosen) GetSkinState(string key, bool defaultOpened, bool defaultChosen)
    {
        using (var connection = new SqliteConnection(ConnectionString))
        {
            connection.Open();
            int skinId = GetOrCreateSkinId(connection, key);
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT opened, chosen FROM PlayerSkins WHERE player_id=@player AND skin_id=@skin";
                command.Parameters.AddWithValue("@player", PlayerId);
                command.Parameters.AddWithValue("@skin", skinId);
                using (IDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        bool opened = reader.GetInt32(0) == 1;
                        bool chosen = reader.GetInt32(1) == 1;
                        return (opened, chosen);
                    }
                }
                command.CommandText = "INSERT INTO PlayerSkins(player_id,skin_id,opened,chosen) VALUES(@player,@skin,@opened,@chosen)";
                command.Parameters.Clear();
                command.Parameters.AddWithValue("@player", PlayerId);
                command.Parameters.AddWithValue("@skin", skinId);
                command.Parameters.AddWithValue("@opened", defaultOpened ? 1 : 0);
                command.Parameters.AddWithValue("@chosen", defaultChosen ? 1 : 0);
                command.ExecuteNonQuery();
                return (defaultOpened, defaultChosen);
            }
        }
    }

    public void UpdateSkinState(string key, bool opened, bool chosen)
    {
        using (var connection = new SqliteConnection(ConnectionString))
        {
            connection.Open();
            int skinId = GetOrCreateSkinId(connection, key);
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "INSERT OR REPLACE INTO PlayerSkins(player_id,skin_id,opened,chosen) VALUES(@player,@skin,@opened,@chosen)";
                command.Parameters.AddWithValue("@player", PlayerId);
                command.Parameters.AddWithValue("@skin", skinId);
                command.Parameters.AddWithValue("@opened", opened ? 1 : 0);
                command.Parameters.AddWithValue("@chosen", chosen ? 1 : 0);
                command.ExecuteNonQuery();
            }
        }
    }

    public void SaveProgress(int money, int level)
    {
        using (var connection = new SqliteConnection(ConnectionString))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "UPDATE PlayerProgress SET money=@money, level=@level WHERE player_id=@player";
                command.Parameters.AddWithValue("@money", money);
                command.Parameters.AddWithValue("@level", Mathf.Max(1, level));
                command.Parameters.AddWithValue("@player", PlayerId);
                command.ExecuteNonQuery();
            }
        }
    }

    public (int money, int level) LoadProgress()
    {
        using (var connection = new SqliteConnection(ConnectionString))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT money, level FROM PlayerProgress WHERE player_id=@player";
                command.Parameters.AddWithValue("@player", PlayerId);
                using (IDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        int money = reader.GetInt32(0);
                        int level = reader.GetInt32(1);
                        return (money, Mathf.Max(1, level));
                    }
                }
            }
        }
        return (0, 1);
    }

    public void ClearAll()
    {
        using (var connection = new SqliteConnection(ConnectionString))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "DELETE FROM PlayerSkins WHERE player_id=@player";
                command.Parameters.AddWithValue("@player", PlayerId);
                command.ExecuteNonQuery();

                command.CommandText = "UPDATE PlayerProgress SET money=0, level=1 WHERE player_id=@player";
                command.Parameters.Clear();
                command.Parameters.AddWithValue("@player", PlayerId);
                command.ExecuteNonQuery();
            }
        }
    }
}
