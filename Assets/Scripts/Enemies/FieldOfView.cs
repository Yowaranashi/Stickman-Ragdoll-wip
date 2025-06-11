using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class FieldOfView : MonoBehaviour
{
    public System.Action OnSeePlayer;
    public System.Action OnPlayerHidden;
    public bool IsPlayerDetected { get; private set; }
    public float VisionAngle { get => _visionAngle; }
    public float VisionRadius { get => _visionRadius; }
    [SerializeField] private float _visionRadius, _detectionRadius;
    [Range(0, 360), SerializeField] private float _visionAngle;
    [SerializeField] private LayerMask _targetMask;
    private Coroutine _checkingRoutine;
    private Vector3 _directionToTarget;
    private void OnEnable()
    {
        IsPlayerDetected = false;
        _checkingRoutine = StartCoroutine(CheckingPlayer());
    }
    private void OnDisable()
    {
        StopCoroutine(_checkingRoutine);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, _visionRadius);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, _detectionRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, _directionToTarget * _visionRadius);
    }
    private IEnumerator CheckingPlayer()
    {
        WaitForSeconds wait = new WaitForSeconds(1f);
        while (true)
        {
            yield return wait;
            CheckPlayerInFoV();
        }
    }
    private void CheckPlayerInFoV()
    {
        Collider2D characterCollider = Physics2D.OverlapCircle(transform.position, _visionRadius, _targetMask);
        if (characterCollider != null)
        {
            if (IsPlayerDetected) return;
            Transform target = characterCollider.transform;
            if (Vector2.Distance(transform.parent.position, target.position) < _detectionRadius)
            {
                OnSeePlayer?.Invoke();
            }
            var directionToTarget = (target.position - transform.position).normalized;
            _directionToTarget = directionToTarget;
            var hittedObject = Physics2D.Raycast(transform.position, directionToTarget, _visionRadius, _targetMask);
            if (hittedObject.transform != null)
            {
                var playerInFoV = Vector2.Angle(transform.right, directionToTarget) < _visionAngle / 2;
                if (hittedObject.transform.TryGetComponent(out Player player) && playerInFoV)
                {
                    IsPlayerDetected = true;
                    OnSeePlayer?.Invoke();
                }
            }
        }
        else if (IsPlayerDetected)
        {
            IsPlayerDetected = false;
            OnPlayerHidden?.Invoke();
        }
    }
}
