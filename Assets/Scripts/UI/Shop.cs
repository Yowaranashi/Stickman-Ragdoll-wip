using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    [SerializeField] private SaveManager _saves;
    [SerializeField] private SkinsDatabase _database;
    [SerializeField] MoneyPanel _moneyPanel;
    [SerializeField] private SelectSkinButton _buttonPrefab;
    [SerializeField] private CharacterCustomization _character;
    private List<SelectSkinButton> _skinButtons;
    private CharacterSkin _selectedSkin;
    private bool _initialized;
    private void OnEnable()
    {
        if(!_initialized)
        {
            _skinButtons = new List<SelectSkinButton>();
            for (int i = 0; i < _database.Skins.Length; i++)
            {
                var button = Instantiate(_buttonPrefab, transform);
                button.Init(_database.Skins[i], this);
                _skinButtons.Add(button);
                if (_database.Skins[i].Chosen)
                    _selectedSkin = _database.Skins[i];
            }
            _initialized = true;
        }
    }
    public void ChangeSkin(CharacterSkin newSkin)
    {
        if (_selectedSkin == null)
            _selectedSkin = _database.GetChosenSkin();
        for (int i = 0; i < _database.Skins.Length; i++)
        {
            if (_database.Skins[i].Key == _selectedSkin.Key)
            {
                _database.Skins[i].Chosen = false;
                _skinButtons[i].DisableSkin();
                break;
            }
        }
        _selectedSkin = newSkin;
        _character.ApplySkin(_selectedSkin);
    }
    public void Save()
    {
        print("DATA SAVED!");
        _saves.SaveSkinsState();
        _saves.SaveMoney();
    }
    public bool CanBuySkin(int cost)
    {
        if(cost <= _database.Money)
        {
            _database.Money -= cost;
            _moneyPanel.UpdateMoneyText();
            return true;
        }
        return false;
    }
}
