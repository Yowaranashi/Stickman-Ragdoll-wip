using UnityEngine;
using TMPro;
using InstantGamesBridge;

public class MoneyPanel : MonoBehaviour
{
    [SerializeField] private SkinsDatabase _database;
    [SerializeField] private CharacterCustomization _characterCustomization;
    [SerializeField] private TextMeshProUGUI _moneyText;
    [SerializeField] private SaveManager _saveManager;
    private void Start()
    {
        Time.timeScale= 1.0f;
        _database.SetKeys();
        _saveManager.DataLoaded += UpdateMoneyText;
        _saveManager.SelectedSkinLoaded += _characterCustomization.ApplySkin;
        _saveManager.LoadData();
    }
    private void OnDisable()
    {
        _saveManager.DataLoaded -= UpdateMoneyText;
        _saveManager.SelectedSkinLoaded -= _characterCustomization.ApplySkin;
    }
    public void UpdateMoneyText()
    {
        print("UPDATE MONEY TEXT");
        _moneyText.text = _database.Money.ToString();
    }
}
