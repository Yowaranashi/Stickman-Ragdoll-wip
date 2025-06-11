using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    [SerializeField] private GameObject _shopPanel, _settingsPanel, _levelsPanel;
    public void OpenShop()
    {
        _shopPanel.SetActive(!_shopPanel.activeSelf);
        _settingsPanel.SetActive(false);
        _levelsPanel.SetActive(false);
    }
    public void OpenLevels()
    {
        _levelsPanel.SetActive(!_levelsPanel.activeSelf);
        _settingsPanel.SetActive(false);
        _shopPanel.SetActive(false);
    }
    public void OpenSettingsPanel()
    {
        _settingsPanel.SetActive(!_settingsPanel.activeSelf);
        _levelsPanel.SetActive(false);
        _shopPanel.SetActive(false);
    }
}
