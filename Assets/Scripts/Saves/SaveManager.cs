using InstantGamesBridge;
using InstantGamesBridge.Modules.Storage;
using System;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    private const string OpenedState = "Open", ChosenState = "Chosen", LevelKey = "LastLevel";
    public Action DataLoaded;
    public Action<int> LevelLoaded;
    public Action<CharacterSkin> SelectedSkinLoaded;    
    [SerializeField] private SkinsDatabase _database;
    public void ClearData()
    {
        var keys = new List<string>();
        foreach (var item in _database.Skins)
        {
            keys.Add(item.Key + OpenedState);
            keys.Add(item.Key + ChosenState);
        }
        keys.Add("Money");
        keys.Add(LevelKey);
        Bridge.storage.Delete(keys, null, StorageType.LocalStorage);
    }
    public void LoadData()
    {
        int openedSkins = 0;
        var openKeys = new List<string>();
        var chosenKeys = new List<string>();
        foreach (var item in _database.Skins)
        {
            openKeys.Add(item.Key + OpenedState);
            chosenKeys.Add(item.Key + ChosenState);
        }
        Bridge.storage.Get(openKeys, (success, data) =>
        {
            if (success)
            {
                if (data != null)
                {
                    for (int i = 0; i < _database.Skins.Length; i++)
                    {
                        var opened = Convert.ToBoolean(data[i]);
                        _database.Skins[i].Opened = opened;
                        if (opened)
                            openedSkins++;
                    }
                    if (openedSkins == 0)
                    {
                        _database.Skins[0].Opened = true;
                        print("0 OPENED SKINS FOUND");
                    }
                }
                else
                {
                    print("OPEN DATA FUCK");
                }
            }
        }, StorageType.LocalStorage);

        Bridge.storage.Get(chosenKeys, (success, data) =>
        {
            if (success)
            {
                if (data != null)
                {
                    for (int i = 0; i < _database.Skins.Length; i++)
                    {
                        _database.Skins[i].Chosen = Convert.ToBoolean(data[i]);
                    }
                    if(openedSkins== 0)
                        _database.Skins[0].Chosen = true;
                    SelectedSkinLoaded?.Invoke(_database.GetChosenSkin());
                }
                else
                {
                    print("CHOSE DATA FUCK");
                }
            }
        }, StorageType.LocalStorage);

        Bridge.storage.Get("Money", (success, data) =>
        {
            if (data != null)
            {
                print($"MONEY DATA {data}");
                _database.Money = Convert.ToInt32(data);
                DataLoaded?.Invoke();
            }
            else
            {
                print("MONEY DATA NOT SET");
            }
        }, StorageType.LocalStorage);
    }
    public void SaveMoney()
    {
        Bridge.storage.Set("Money", _database.Money, null, StorageType.LocalStorage);
    }
    public void SaveLevel(int level)
    {
        Bridge.storage.Set(LevelKey, level, null, StorageType.LocalStorage);
    }
    public int LoadLevel()
    {
        int level = 1;
        Bridge.storage.Get(LevelKey, (success, data) =>
        {
            if (data != null)
            {
                print($"LEVEL DATA {data}");
                level = Convert.ToInt32(data);
                LevelLoaded?.Invoke(Convert.ToInt32(data));
            }
            else
            {
                LevelLoaded?.Invoke(1);
                print("LEVEL DATA NOT SET");
            }
        }, StorageType.LocalStorage);
        //LevelLoaded?.Invoke(level);
        return level;
    }
    public void SaveSkinsState()
    {
        var openKeys = new List<string>();
        var openValues = new List<object>();
        var chosenKeys = new List<string>();
        var chosenValues = new List<object>();
        foreach (var item in _database.Skins)
        {
            openKeys.Add(item.Key + OpenedState);
            openValues.Add(item.Opened);
            chosenKeys.Add(item.Key + ChosenState);
            chosenValues.Add(item.Chosen);
        }
        Bridge.storage.Set(openKeys, openValues, null, StorageType.LocalStorage);
        Bridge.storage.Set(chosenKeys, chosenValues, null, StorageType.LocalStorage);
    }
}
