using UnityEngine;
using System.Data;
using System.IO;
using Mono.Data.Sqlite;

public class DatabaseManager
{
    private const string DatabaseName = "GameData.db";

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
                command.CommandText = "CREATE TABLE IF NOT EXISTS Skins(key TEXT PRIMARY KEY, opened INTEGER NOT NULL, chosen INTEGER NOT NULL);";
                command.ExecuteNonQuery();
                command.CommandText = "CREATE TABLE IF NOT EXISTS Progress(id INTEGER PRIMARY KEY CHECK(id = 1), money INTEGER NOT NULL, level INTEGER NOT NULL);";
                command.ExecuteNonQuery();
                command.CommandText = "INSERT OR IGNORE INTO Progress(id,money,level) VALUES (1,0,1);";
                command.ExecuteNonQuery();
            }
        }
    }

    public (bool opened, bool chosen) GetSkinState(string key, bool defaultOpened, bool defaultChosen)
    {
        using (var connection = new SqliteConnection(ConnectionString))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT opened, chosen FROM Skins WHERE key=@key";
                command.Parameters.AddWithValue("@key", key);
                using (IDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        bool opened = reader.GetInt32(0) == 1;
                        bool chosen = reader.GetInt32(1) == 1;
                        return (opened, chosen);
                    }
                }
                command.CommandText = "INSERT OR REPLACE INTO Skins(key,opened,chosen) VALUES(@key,@opened,@chosen)";
                command.Parameters.Clear();
                command.Parameters.AddWithValue("@key", key);
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
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "INSERT OR REPLACE INTO Skins(key,opened,chosen) VALUES(@key,@opened,@chosen)";
                command.Parameters.AddWithValue("@key", key);
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
                command.CommandText = "UPDATE Progress SET money=@money, level=@level WHERE id=1";
                command.Parameters.AddWithValue("@money", money);
                command.Parameters.AddWithValue("@level", level);
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
                command.CommandText = "SELECT money, level FROM Progress WHERE id=1";
                using (IDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        int money = reader.GetInt32(0);
                        int level = reader.GetInt32(1);
                        return (money, level);
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
                command.CommandText = "DELETE FROM Skins";
                command.ExecuteNonQuery();
                command.CommandText = "UPDATE Progress SET money=0, level=1 WHERE id=1";
                command.ExecuteNonQuery();
            }
        }
    }
}
