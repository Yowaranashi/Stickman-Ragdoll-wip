using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Finish : MonoBehaviour
{
    [SerializeField] private SaveManager _saveManager;
    [SerializeField] private int _winMoney = 100;
    [SerializeField] private SkinsDatabase _database;
    [SerializeField] private FinishPanel _finishPanel;
    [SerializeField] private Collider2D _deathCollider;
    [SerializeField] private ParticleSystem _finishParticles;
    [SerializeField] private IntersititalAd _interstitialAd;
    private Player _player;
    private void Start()
    {
        //если вдруг забыл добавить
        if(_interstitialAd == null)
            _interstitialAd = FindObjectOfType<IntersititalAd>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out Player player))
        {
            _player = player;
            player.Finish();
            var lastLevel = _saveManager.LoadLevel();
            if(lastLevel < SceneManager.GetActiveScene().buildIndex + 1)
            {
                _saveManager.SaveLevel(SceneManager.GetActiveScene().buildIndex + 1);
            }
            _database.Money += _winMoney;
            _deathCollider.enabled = false;
            _saveManager.SaveMoney();
            StartCoroutine(ShowFinishPanel());
        }
    }
    private IEnumerator ShowFinishPanel()
    {
        Time.timeScale = 0.75f;
        yield return new WaitForSeconds(.5f);
        _player.StopFollowCamera();
        if(_interstitialAd != null)
            _interstitialAd.ShowInterstitial();
        _finishPanel.gameObject.SetActive(true);
        _finishParticles.gameObject.SetActive(true);
        Time.timeScale = 1f;
    }
}
