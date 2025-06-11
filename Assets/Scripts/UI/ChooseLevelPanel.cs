using UnityEngine;

public class ChooseLevelPanel : MonoBehaviour
{
    [SerializeField] private SceneSwitcher _sceneSwitcher;
    [SerializeField] private SaveManager _saveManager;
    [SerializeField] private ChooseLevelButton _buttonPrefab;
    [SerializeField] private int _firstSceneIndex = 1, _lastSceneIndex = 17;
    private bool _initialized;
    private void OnEnable()
    {
        if (_initialized) return;
        _saveManager.LevelLoaded += InitPanel;
        _saveManager.LoadLevel();
    }
    private void InitPanel(int lastLevelIndex)
    {
        //if(lastLevelIndex == 0) lastLevelIndex = 1;
        for (int i = _firstSceneIndex; i < _lastSceneIndex + 1; i++)
        {
            print("INIT BTN " + lastLevelIndex);
            var btn = Instantiate(_buttonPrefab, transform);
            btn.InitButton(i, _sceneSwitcher, lastLevelIndex);
        }
        _saveManager.LevelLoaded -= InitPanel;
        _initialized = true;
    }
}
