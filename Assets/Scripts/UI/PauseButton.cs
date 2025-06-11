using UnityEngine;

public class PauseButton : MonoBehaviour
{
    [SerializeField] private GameObject _pausePanel;
    private bool _paused;
    public void Pause()
    {
        _paused = !_paused;
        Time.timeScale = _paused? 0.0f : 1.0f;
        _pausePanel.SetActive(_paused);
    }
}
