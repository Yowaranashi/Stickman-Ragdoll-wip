using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] protected ObjectsPool _bulletsPool;
    [SerializeField] private Transform _shotPoint;
    [SerializeField] private float _shotBulletSpeed = 2f, _shootingSpeed = 1f, _spread = .5f;
    [SerializeField] private int _amountOfBullets = 1;
    [SerializeField] private float _timeBetweenShooting = 0;
    public float ShootingSpeed { get => _shootingSpeed; }
    public void Shot()
    {
        for (int i = 0; i < _amountOfBullets; i++)
        {
            Invoke(nameof(CreateBullet), _timeBetweenShooting*i);
        }
    }
    private void CreateBullet()
    {
        var bullet = _bulletsPool.GetObject(_shotPoint.position, Quaternion.identity).GetComponent<Bullet>();
        Vector2 pdir = Vector2.Perpendicular(-_shotPoint.up) * Random.Range(-_spread, _spread);
        bullet.ShotBullet(-(Vector2)_shotPoint.up + pdir, _shotBulletSpeed);
    }
}
