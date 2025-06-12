using UnityEngine;
using UnityEngine.SceneManagement;
using static Cinemachine.DocumentationSortingAttribute;

public class SceneSwitcher : MonoBehaviour
{
    public int LastLevel;
    public void ToMainMenu()
    {
        SceneManager.LoadScene(1);
    }
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void Open(int level)
    {
        SceneManager.LoadScene(level);
    }
    public void SwitchToNext()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
