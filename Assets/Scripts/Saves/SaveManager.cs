using System;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    private const string OpenedState = "Open";
    private const string ChosenState = "Chosen";
    private const string LevelKey = "LastLevel";

    public Action DataLoaded;
    public Action<int> LevelLoaded;
    public Action<CharacterSkin> SelectedSkinLoaded;
    [SerializeField] private SkinsDatabase _database;

    public void ClearData()
    {
        foreach (var item in _database.Skins)
        {
            item.Opened = false;
            item.Chosen = false;
        }
        _database.Money = 100;
        DatabaseManager.Instance.SaveProgress(_database.Money, 1);
        DatabaseManager.Instance.SaveSkins(_database.Skins);
    }

    public void LoadData()
    {
        var skinsData = DatabaseManager.Instance.LoadSkins();
        int openedSkins = 0;
        for (int i = 0; i < _database.Skins.Length; i++)
        {
            if (skinsData.TryGetValue(_database.Skins[i].Key, out var state))
            {
                _database.Skins[i].Opened = state.opened == 1;
                _database.Skins[i].Chosen = state.chosen == 1;
            }
            if (_database.Skins[i].Opened)
                openedSkins++;
        }

        if (openedSkins == 0)
            _database.Skins[0].Opened = true;

        var progress = DatabaseManager.Instance.LoadProgress();
        _database.Money = progress.money;
        SelectedSkinLoaded?.Invoke(_database.GetChosenSkin());
        DataLoaded?.Invoke();
    }

    public void SaveMoney()
    {
        DatabaseManager.Instance.SaveProgress(_database.Money, LoadLevel());
    }

    public void SaveLevel(int level)
    {
        DatabaseManager.Instance.SaveProgress(_database.Money, level);
    }

    public int LoadLevel()
    {
        var progress = DatabaseManager.Instance.LoadProgress();
        LevelLoaded?.Invoke(progress.level);
        return progress.level;
    }

    public void SaveSkinsState()
    {
        DatabaseManager.Instance.SaveSkins(_database.Skins);
    }
}
