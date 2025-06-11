using System.Collections;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private DirectionChecker directionChecker;
    [SerializeField] private GroundChecker _groundChecker;
    [SerializeField] private WallsChecker _wallsChecker;
    [SerializeField] private Rigidbody2D _body;
    [SerializeField] private float _climbingSpeed = 2f;
    [SerializeField] Rigidbody2D _leftLegRB;
    [SerializeField] Rigidbody2D _rightLegRB;
    [SerializeField] float speed = 2f;
    [SerializeField] float legWait = .5f;
    //[SerializeField] private Collider2D _legRCollider, _legLCollider, _armRCollider, _armLCollider;
    private bool _moveRight;
    private Animator _animator;
    private void Start()
    {
        _moveRight = true;
        _animator = GetComponent<Animator>();
    }
    private void OnEnable()
    {
        _wallsChecker.Collided += ResetVelocity;
    }
    private void OnDisable()
    {
        _wallsChecker.Collided -= ResetVelocity;
    }
    // Update is called once per frame
    private void Update()
    {
        if(_groundChecker.Grounded && _wallsChecker.Climbing)
        {
            _animator.Play("Climb");
            //_legRCollider.isTrigger = true;
            //_legLCollider.isTrigger = true;
            //_armRCollider.isTrigger = true;
            //_armLCollider.isTrigger = true;
            _body.AddForce(Vector2.up * _climbingSpeed);
            _body.linearVelocity = new Vector2(0, _body.linearVelocity.y);
        }
        else if (_groundChecker.Grounded)
        {
            _animator.Play("WalkRight");
            StartCoroutine(MoveRight(legWait));
        }
        else
        {
            _animator.Play("idle");
        }
    }
    private void ResetVelocity()
    {
        _body.linearVelocity = Vector2.zero;
    }
    IEnumerator MoveRight(float seconds)
    {
        Vector2 moveDir = _moveRight ? Vector2.right : Vector2.left;
        _leftLegRB.AddForce(moveDir * (speed*1000) * Time.deltaTime);
        yield return new WaitForSeconds(seconds);
        _rightLegRB.AddForce(moveDir * (speed * 1000) * Time.deltaTime);
    }
}
