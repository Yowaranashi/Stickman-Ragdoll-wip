using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody2D _bulletRb;
    private ObjectsPool _effectsPool;
    private TrailRenderer _trailRenderer;
    public ObjectsPool EffectsPool { get => _effectsPool; set => _effectsPool = value; }

    private void Start()
    {
        _bulletRb = GetComponent<Rigidbody2D>();
    }
    private void OnEnable()
    {
        if(_bulletRb == null)
            _bulletRb= GetComponent<Rigidbody2D>();
        if(_trailRenderer == null)
            _trailRenderer= GetComponent<TrailRenderer>();
        StartCoroutine(DisableBulletAfterDelay());
        StartCoroutine(ActivateTrailAfterDelay());
    }
    private void OnDisable()
    {
        _trailRenderer.enabled = false;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(_effectsPool != null)
            _effectsPool.GetObject(transform.position, Quaternion.identity);
        if(collision.transform.TryGetComponent(out Rigidbody2D rb))
            rb.AddForce(collision.transform.right * -100, ForceMode2D.Impulse);
        gameObject.SetActive(false);
    }
    public void ShotBullet(Vector2 shotDirection, float bulletSpeed)
    {
        _bulletRb.AddForce(shotDirection * bulletSpeed, ForceMode2D.Impulse);
    }
    private IEnumerator DisableBulletAfterDelay()
    {
        yield return new WaitForSeconds(2f);
        gameObject.SetActive(false);
    }
    private IEnumerator ActivateTrailAfterDelay()
    {
        yield return new WaitForSeconds(.1f);
        _trailRenderer.enabled = true;
    }
}
