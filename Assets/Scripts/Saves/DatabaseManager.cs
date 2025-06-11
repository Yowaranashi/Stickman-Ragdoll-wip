using System;
using System.Collections.Generic;
using System.Data;
using Mono.Data.Sqlite;
using UnityEngine;

public class DatabaseManager : MonoBehaviour
{
    public static DatabaseManager Instance { get; private set; }
    public int CurrentUserId { get; private set; }

    private string _dbPath;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            _dbPath = "URI=file:" + Application.persistentDataPath + "/game.db";
            Init();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Init()
    {
        using (var connection = new SqliteConnection(_dbPath))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "CREATE TABLE IF NOT EXISTS Users (id INTEGER PRIMARY KEY AUTOINCREMENT, username TEXT UNIQUE, password TEXT);";
                command.ExecuteNonQuery();

                command.CommandText = "CREATE TABLE IF NOT EXISTS Progress (user_id INTEGER PRIMARY KEY, money INTEGER DEFAULT 100, level INTEGER DEFAULT 1, FOREIGN KEY(user_id) REFERENCES Users(id) ON DELETE CASCADE);";
                command.ExecuteNonQuery();

                command.CommandText = "CREATE TABLE IF NOT EXISTS Skins (user_id INTEGER, skin_key TEXT, opened INTEGER DEFAULT 0, chosen INTEGER DEFAULT 0, PRIMARY KEY(user_id, skin_key), FOREIGN KEY(user_id) REFERENCES Users(id) ON DELETE CASCADE);";
                command.ExecuteNonQuery();
            }
        }
    }

    public bool Register(string username, string password)
    {
        try
        {
            using (var connection = new SqliteConnection(_dbPath))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "INSERT INTO Users(username,password) VALUES(@u,@p);";
                    command.Parameters.Add(new SqliteParameter("@u", username));
                    command.Parameters.Add(new SqliteParameter("@p", password));
                    command.ExecuteNonQuery();
                    command.CommandText = "SELECT last_insert_rowid();";
                    command.Parameters.Clear();
                    long id = (long)command.ExecuteScalar();
                    command.Parameters.Clear();
                    command.CommandText = "INSERT INTO Progress(user_id) VALUES(@id);";
                    command.Parameters.Add(new SqliteParameter("@id", id));
                    command.ExecuteNonQuery();
                    CurrentUserId = (int)id;
                    return true;
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Registration error: " + e.Message);
            return false;
        }
    }

    public bool Login(string username, string password)
    {
        using (var connection = new SqliteConnection(_dbPath))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT id FROM Users WHERE username=@u AND password=@p";
                command.Parameters.Add(new SqliteParameter("@u", username));
                command.Parameters.Add(new SqliteParameter("@p", password));
                var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    CurrentUserId = reader.GetInt32(0);
                    return true;
                }
            }
        }
        return false;
    }

    public (int money, int level) LoadProgress()
    {
        using (var connection = new SqliteConnection(_dbPath))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT money, level FROM Progress WHERE user_id=@id";
                command.Parameters.Add(new SqliteParameter("@id", CurrentUserId));
                var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    return (reader.GetInt32(0), reader.GetInt32(1));
                }
            }
        }
        return (100, 1);
    }

    public void SaveProgress(int money, int level)
    {
        using (var connection = new SqliteConnection(_dbPath))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "INSERT OR REPLACE INTO Progress(user_id,money,level) VALUES(@id,@m,@l);";
                command.Parameters.Add(new SqliteParameter("@id", CurrentUserId));
                command.Parameters.Add(new SqliteParameter("@m", money));
                command.Parameters.Add(new SqliteParameter("@l", level));
                command.ExecuteNonQuery();
            }
        }
    }

    public Dictionary<string, (int opened, int chosen)> LoadSkins()
    {
        var skins = new Dictionary<string, (int, int)>();
        using (var connection = new SqliteConnection(_dbPath))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT skin_key, opened, chosen FROM Skins WHERE user_id=@id";
                command.Parameters.Add(new SqliteParameter("@id", CurrentUserId));
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var key = reader.GetString(0);
                    var opened = reader.GetInt32(1);
                    var chosen = reader.GetInt32(2);
                    skins[key] = (opened, chosen);
                }
            }
        }
        return skins;
    }

    public void SaveSkins(CharacterSkin[] skins)
    {
        using (var connection = new SqliteConnection(_dbPath))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                foreach (var skin in skins)
                {
                    command.Parameters.Clear();
                    command.CommandText = "INSERT OR REPLACE INTO Skins(user_id,skin_key,opened,chosen) VALUES(@id,@k,@o,@c);";
                    command.Parameters.Add(new SqliteParameter("@id", CurrentUserId));
                    command.Parameters.Add(new SqliteParameter("@k", skin.Key));
                    command.Parameters.Add(new SqliteParameter("@o", skin.Opened ? 1 : 0));
                    command.Parameters.Add(new SqliteParameter("@c", skin.Chosen ? 1 : 0));
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
