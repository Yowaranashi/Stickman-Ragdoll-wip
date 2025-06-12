using UnityEngine;

public class ChooseLevelPanel : MonoBehaviour
{
    [SerializeField] private SceneSwitcher _sceneSwitcher;
    [SerializeField] private SaveManager _saveManager;
    [SerializeField] private ChooseLevelButton _buttonPrefab;

    [Tooltip("Build Index первой сцены уровня (Tutorial)")]
    [SerializeField] private int _firstLevelBuildIndex = 2;

    [Tooltip("Общее количество уровней (включая Tutorial)")]
    [SerializeField] private int _levelsCount = 16;

    private bool _initialized;

    private void OnEnable()
    {
        if (_initialized) return;
        _saveManager.LevelLoaded += InitPanel;
        _saveManager.LoadLevel();
    }

    private void InitPanel(int lastLevelIndex)
    {
        if (lastLevelIndex == 0) lastLevelIndex = 1;

        for (int levelId = 1; levelId <= _levelsCount; levelId++)
        {
            int buildIndex = _firstLevelBuildIndex + (levelId - 1);

            var btn = Instantiate(_buttonPrefab, transform);
            btn.InitButton(levelId, buildIndex, _sceneSwitcher, lastLevelIndex);
        }

        _saveManager.LevelLoaded -= InitPanel;
        _initialized = true;
    }
}
