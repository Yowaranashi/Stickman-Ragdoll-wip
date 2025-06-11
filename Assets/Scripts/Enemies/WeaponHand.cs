using System.Collections;
using UnityEngine;

public class WeaponHand : MonoBehaviour
{
    [SerializeField] private Transform _gun;
    [SerializeField] private Weapon _weapon;
    [SerializeField] private FieldOfView _fieldOfView;
    private Transform _target;
    private Balance _balance;
    private Vector2 _playerDir;
    private Coroutine _shooting;
    private void Start()
    {
        _balance = GetComponent<Balance>();
        _target = FindObjectOfType<Player>().transform;
    }
    private void OnEnable()
    {
        _fieldOfView.OnSeePlayer += Shot;
    }
    private void OnDisable()
    {
        _fieldOfView.OnSeePlayer -= Shot;
    }
    private void Update()
    {
        _playerDir = _gun.position - _target.position;
        float angle = Mathf.Atan2(_playerDir.y, _playerDir.x) * Mathf.Rad2Deg;
       _balance.TargetRotation = angle;
    }
    public void Shot()
    {
        print("SHOOTIUNG");
        _shooting = StartCoroutine(Shooting());
    }
    public void StopShooting()
    {
        StopCoroutine(_shooting);
    }
    private IEnumerator Shooting()
    {
        print("PLAYER DETECTED " + _fieldOfView.IsPlayerDetected);
        while (_fieldOfView.IsPlayerDetected)
        {
            yield return new WaitForSeconds(_weapon.ShootingSpeed);
            _weapon.Shot();
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(_gun.position, _playerDir*3);
        Gizmos.color = Color.yellow;
    }
}
