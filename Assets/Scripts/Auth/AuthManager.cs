using System;
using Mono.Data.Sqlite;
using UnityEngine;

public class AuthManager : MonoBehaviour
{
    public Action<int> LoggedIn;

    private void Awake()
    {
        DatabaseManager.Initialize();
    }

    public void Register(string username, string password)
    {
        using (var cmd = DatabaseManager.Connection.CreateCommand())
        {
            cmd.CommandText = "INSERT INTO Accounts(username, password) VALUES (@u,@p);";
            cmd.Parameters.Add(new SqliteParameter("@u", username));
            cmd.Parameters.Add(new SqliteParameter("@p", password));
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
            }
        }
    }

    public bool Login(string username, string password)
    {
        using (var cmd = DatabaseManager.Connection.CreateCommand())
        {
            cmd.CommandText = "SELECT id FROM Accounts WHERE username=@u AND password=@p;";
            cmd.Parameters.Add(new SqliteParameter("@u", username));
            cmd.Parameters.Add(new SqliteParameter("@p", password));
            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    int id = reader.GetInt32(0);
                    LoggedIn?.Invoke(id);
                    return true;
                }
            }
        }
        return false;
    }

    private void OnApplicationQuit()
    {
        DatabaseManager.Close();
    }
}
