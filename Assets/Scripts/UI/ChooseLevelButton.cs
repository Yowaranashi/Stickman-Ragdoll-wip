using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChooseLevelButton : MonoBehaviour
{
    [SerializeField] private Sprite _notOpenLayout;

    public void InitButton(int levelId, int buildIndex, SceneSwitcher switcher, int lastLevelIndex)
    {
        var txt = GetComponentInChildren<TextMeshProUGUI>();
        txt.text = levelId.ToString();

        var btn = GetComponent<Button>();
        var img = GetComponent<Image>();

        if (levelId > lastLevelIndex)
        {
            btn.interactable = false;
            btn.onClick.RemoveAllListeners();
            img.sprite = _notOpenLayout;
        }
        else
        {
            btn.onClick.AddListener(() => switcher.Open(buildIndex));
        }
    }
}
