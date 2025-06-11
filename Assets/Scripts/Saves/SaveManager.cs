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
    private DatabaseManager _db;

    private void Awake()
    {
        _db = new DatabaseManager();
    }

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
        _db.ClearAll();
    }

    public void LoadData()
    {
        int openedSkins = 0;
        for (int i = 0; i < _database.Skins.Length; i++)
        {
            var state = _db.GetSkinState(_database.Skins[i].Key, i == 0, i == 0);
            _database.Skins[i].Opened = state.opened;
            _database.Skins[i].Chosen = state.chosen;
            if (state.opened)
                openedSkins++;
        }

        if (openedSkins == 0)
            _database.Skins[0].Opened = true;

        var progress = _db.LoadProgress();
        _database.Money = progress.money;

        SelectedSkinLoaded?.Invoke(_database.GetChosenSkin());
        DataLoaded?.Invoke();
    }

    public void SaveMoney()
    {
        var progress = _db.LoadProgress();
        _db.SaveProgress(_database.Money, progress.level);
    }

    public void SaveLevel(int level)
    {
        var progress = _db.LoadProgress();
        _db.SaveProgress(progress.money, Mathf.Max(1, level));
    }

    public int LoadLevel()
    {
        var progress = _db.LoadProgress();
        LevelLoaded?.Invoke(progress.level);
        return progress.level;
    }

    public void SaveSkinsState()
    {
        for (int i = 0; i < _database.Skins.Length; i++)
        {
            _db.UpdateSkinState(_database.Skins[i].Key, _database.Skins[i].Opened, _database.Skins[i].Chosen);
        }
    }
}
