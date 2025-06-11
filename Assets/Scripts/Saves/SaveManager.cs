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
            PlayerPrefs.DeleteKey(item.Key + OpenedState);
            PlayerPrefs.DeleteKey(item.Key + ChosenState);
        }
        PlayerPrefs.DeleteKey("Money");
        PlayerPrefs.DeleteKey(LevelKey);
        PlayerPrefs.Save();
    }

    public void LoadData()
    {
        int openedSkins = 0;
        for (int i = 0; i < _database.Skins.Length; i++)
        {
            var key = _database.Skins[i].Key + OpenedState;
            bool opened = PlayerPrefs.GetInt(key, i == 0 ? 1 : 0) == 1;
            _database.Skins[i].Opened = opened;
            if (opened)
                openedSkins++;
        }

        for (int i = 0; i < _database.Skins.Length; i++)
        {
            var key = _database.Skins[i].Key + ChosenState;
            bool chosen = PlayerPrefs.GetInt(key, i == 0 ? 1 : 0) == 1;
            _database.Skins[i].Chosen = chosen;
        }

        if (openedSkins == 0)
            _database.Skins[0].Opened = true;

        SelectedSkinLoaded?.Invoke(_database.GetChosenSkin());

        _database.Money = PlayerPrefs.GetInt("Money", _database.Money);
        DataLoaded?.Invoke();
    }

    public void SaveMoney()
    {
        PlayerPrefs.SetInt("Money", _database.Money);
        PlayerPrefs.Save();
    }

    public void SaveLevel(int level)
    {
        PlayerPrefs.SetInt(LevelKey, level);
        PlayerPrefs.Save();
    }

    public int LoadLevel()
    {
        int level = PlayerPrefs.GetInt(LevelKey, 1);
        LevelLoaded?.Invoke(level);
        return level;
    }

    public void SaveSkinsState()
    {
        for (int i = 0; i < _database.Skins.Length; i++)
        {
            PlayerPrefs.SetInt(_database.Skins[i].Key + OpenedState, _database.Skins[i].Opened ? 1 : 0);
            PlayerPrefs.SetInt(_database.Skins[i].Key + ChosenState, _database.Skins[i].Chosen ? 1 : 0);
        }
        PlayerPrefs.Save();
    }
}
