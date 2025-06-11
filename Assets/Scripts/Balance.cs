using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Balance : MonoBehaviour
{
    public float Force;
    [SerializeField] private float _targetRotation, _minRotation, _maxRotation;
    private Rigidbody2D _rb;
    [SerializeField] private bool _useLimits = false;
    public float TargetRotation { 
        get => _targetRotation;
        set
        {
            if(_useLimits)
                _targetRotation = Mathf.Clamp(value, _minRotation, _maxRotation);
            else
                _targetRotation = value;
        }
    }
    public Collider2D Collider { get; private set; }
    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }
    private void OnEnable()
    {
        if(Collider == null)
            Collider = GetComponent<Collider2D>();
    }
    private void Update()
    {
        _rb.MoveRotation(Mathf.LerpAngle(_rb.rotation, _targetRotation, Force * Time.deltaTime));
    }
}
