using UnityEngine;

public class Tutorial : MonoBehaviour
{
    [SerializeField] private GameObject _finger;
    [SerializeField] private GrapplingGun _grapplingGun;
    private void Start()
    {
        Time.timeScale = .75f;
        _grapplingGun.StartGrapple += StopTutorial;
    }
    private void StopTutorial()
    {
        _finger.SetActive(false);
        Time.timeScale = 1;
        _grapplingGun.StartGrapple -= StopTutorial;
        enabled = false;
    }
}
