using TMPro;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChooseLevelButton : MonoBehaviour
{
    private TextMeshProUGUI _btnText;
    [SerializeField] private Sprite _notOpenLayout;
    public void InitButton(int levelId, SceneSwitcher switcher, int lastLevelIndex)
    {
        _btnText = GetComponentInChildren<TextMeshProUGUI>();
        _btnText.text = levelId.ToString();
        var btn = GetComponent<Button>();

        int buildIndex = SceneUtility.GetBuildIndexByScenePath($"Assets/Scenes/Level {levelId}.unity");
        if (buildIndex == -1 || buildIndex > lastLevelIndex)
        {
            btn.enabled = false;
            GetComponent<Image>().sprite = _notOpenLayout;
        }

        btn.onClick.AddListener(delegate
        {
            switcher.Open(levelId);
        });
    }
}
