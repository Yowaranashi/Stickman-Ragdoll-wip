using System.Collections.Generic;
using Mono.Data.Sqlite;

public static class AccountProgress
{
    public static void SaveLevel(int accountId, int levelId)
    {
        using (var cmd = DatabaseManager.Connection.CreateCommand())
        {
            cmd.CommandText = "INSERT OR IGNORE INTO AccountLevels(account_id, level_id) VALUES(@a,@l);";
            cmd.Parameters.Add(new SqliteParameter("@a", accountId));
            cmd.Parameters.Add(new SqliteParameter("@l", levelId));
            cmd.ExecuteNonQuery();
        }
    }

    public static void SaveSkin(int accountId, int skinId)
    {
        using (var cmd = DatabaseManager.Connection.CreateCommand())
        {
            cmd.CommandText = "INSERT OR IGNORE INTO AccountSkins(account_id, skin_id) VALUES(@a,@s);";
            cmd.Parameters.Add(new SqliteParameter("@a", accountId));
            cmd.Parameters.Add(new SqliteParameter("@s", skinId));
            cmd.ExecuteNonQuery();
        }
    }

    public static List<int> LoadLevels(int accountId)
    {
        var list = new List<int>();
        using (var cmd = DatabaseManager.Connection.CreateCommand())
        {
            cmd.CommandText = "SELECT level_id FROM AccountLevels WHERE account_id=@a;";
            cmd.Parameters.Add(new SqliteParameter("@a", accountId));
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                    list.Add(reader.GetInt32(0));
            }
        }
        return list;
    }

    public static List<int> LoadSkins(int accountId)
    {
        var list = new List<int>();
        using (var cmd = DatabaseManager.Connection.CreateCommand())
        {
            cmd.CommandText = "SELECT skin_id FROM AccountSkins WHERE account_id=@a;";
            cmd.Parameters.Add(new SqliteParameter("@a", accountId));
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                    list.Add(reader.GetInt32(0));
            }
        }
        return list;
    }
}
