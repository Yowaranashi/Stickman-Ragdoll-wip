using UnityEngine;
public class GrapplingGun : MonoBehaviour
{
    public bool HasGrappleObject = false;
    public System.Action StartGrapple;
    [SerializeField] private Transform _body;
    [SerializeField] private Movement _movement;
    [SerializeField] private GrapplingRope _grappleRope;
    [SerializeField] private bool _grappleToAll = false;
    [SerializeField] private LayerMask _grappleLayerMask;
    [SerializeField] private Camera _camera;
    [Header("Grapple Parts")]
    [SerializeField] private Transform _gunHolder;
    [SerializeField] private Transform _gunPivot;
    [SerializeField] private Transform _firePoint;
    [Header("Physics")]
    [SerializeField] private SpringJoint2D _springJoint2D;
    [SerializeField] private Rigidbody2D _rigidbody;
    [Header("Rotation Settings")]
    [SerializeField] private bool _rotateOverTime = true;
    [Range(0, 60)][SerializeField] private float _rotationSpeed = 4;
    [Header("Distance")]
    [SerializeField] private bool _hasMaxDistance = false;
    [SerializeField] private float _grappleDistance = 20;
    private Rigidbody2D _grappleBody;
    private enum LaunchType
    {
        Transform_Launch,
        Physics_Launch
    }
    [Header("Launching")]
    [SerializeField] private bool _launchToPoint = true;
    [SerializeField] private LaunchType _launchType = LaunchType.Physics_Launch;
    [SerializeField] private float _launchSpeed = 1;

    [Header("No Launch To Point")]
    [SerializeField] private bool autoConfigureDistance = false;
    [SerializeField] private float targetDistance = 3;
    [SerializeField] private float targetFrequncy = 1;
    public Vector2 GrapplePoint { get; private set; }
    public Vector2 GrappleDistanceVector { get; private set; }
    public Transform FirePoint { get => _firePoint; }
    private bool _mobileInput;
    private void Start()
    {
        _mobileInput = Application.isMobilePlatform;
        _grappleRope.enabled = false;
        _springJoint2D.enabled = false;
    }
    private void Update()
    {
        if (!_mobileInput)
            MouseControl();
        else
            MobileControl();
    }
    #region Control
    private void MouseControl()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            SetGrapplePoint(Input.mousePosition);
        }
        else if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            _grappleRope.enabled = false;
            _springJoint2D.enabled = false;
            _rigidbody.gravityScale = 1;
        }
    }
    private void MobileControl()
    {
        if (Input.touchCount == 0)
        {
            return;
        }
        var touch = Input.GetTouch(0);
        if (touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved)
        {
            return;
        }
        if (touch.phase == TouchPhase.Began && !HasGrappleObject) 
        {
            var tPos = new Vector3(touch.rawPosition.x, touch.rawPosition.y, 10);
            SetGrapplePoint(tPos);
        }
        else if (touch.phase == TouchPhase.Ended)
        {
            _grappleRope.enabled = false;
            HasGrappleObject= false;
            _springJoint2D.enabled = false;
            _rigidbody.gravityScale = 1;
        }
    }
    #endregion
    private void SetGrapplePoint(Vector3 touchPosition)
    {
        //���������, ��������� �� ������, �� ������� ����� ����������, ��� ������� ����
        Vector2 cubeRay = _camera.ScreenToWorldPoint(touchPosition);
        RaycastHit2D cubeHit = Physics2D.Raycast(cubeRay, Vector2.zero, _grappleDistance, _grappleLayerMask);
        if (cubeHit)
        {
            HasGrappleObject = true;
            if (cubeHit.collider.isTrigger)
            {
                GrapplePoint = _camera.ScreenToWorldPoint(touchPosition);
            }
            else
            {
                Vector2 distanceVector = _camera.ScreenToWorldPoint(touchPosition) - _gunPivot.position;
                RaycastHit2D[] hits = Physics2D.RaycastAll(_firePoint.position, distanceVector.normalized, _grappleDistance, _grappleLayerMask);
                RaycastHit2D hit = new RaycastHit2D();
                foreach (var h in hits)
                {
                    if(cubeHit.collider == h.collider)
                    {
                        hit = h;
                        break;
                    }
                }
                GrapplePoint = hit.point;
                GrappleDistanceVector = GrapplePoint - (Vector2)_gunPivot.position;
            }
        }
        //���� �� �������, ������� ������� �� �����������, ���� ��� �� ��������� �� �������
        else
        {
            Vector2 distanceVector = _camera.ScreenToWorldPoint(touchPosition) - _firePoint.position;
            if (Physics2D.RaycastAll(_firePoint.position, distanceVector.normalized, _grappleDistance, _grappleLayerMask).Length > 0)
            {
                RaycastHit2D[] hits = Physics2D.RaycastAll(_firePoint.position, distanceVector.normalized, _grappleDistance, _grappleLayerMask);

                RaycastHit2D hit = hits.Length > 1 ? hits[1] : hits[0]; 

                if (Vector2.Distance(hit.point, _firePoint.position) <= _grappleDistance || !_hasMaxDistance)
                {
                    HasGrappleObject = true;
                    GrapplePoint = hit.point;
                    GrappleDistanceVector = GrapplePoint - (Vector2)_firePoint.position;
                }
            }
            else
            {
                HasGrappleObject = false;
                _grappleBody = null;
                Vector2 a = _camera.ScreenToWorldPoint(touchPosition);
                GrapplePoint = a;
                GrappleDistanceVector = GrapplePoint - (Vector2)FirePoint.position;
            }
        }
        _grappleRope.enabled = true;
    }

    public void Grapple()
    {
        StartGrapple?.Invoke();
        _springJoint2D.autoConfigureDistance = false;
        if (!_launchToPoint && !autoConfigureDistance)
        {
            _springJoint2D.distance = targetDistance;
            _springJoint2D.frequency = targetFrequncy;
        }
        if (!_launchToPoint)
        {
            if (autoConfigureDistance)
            {
                _springJoint2D.autoConfigureDistance = true;
                _springJoint2D.frequency = 0;
            }
            _springJoint2D.connectedAnchor = GrapplePoint;
            _springJoint2D.enabled = true;
        }
        else
        {
            switch (_launchType)
            {
                case LaunchType.Physics_Launch:
                    Vector2 distanceVector = _firePoint.position - _gunHolder.position;
                    _springJoint2D.distance = distanceVector.magnitude;
                    _springJoint2D.frequency = _launchSpeed;
                    _springJoint2D.enabled = true;
                    break;
                case LaunchType.Transform_Launch:
                    _rigidbody.gravityScale = 0;
                    _rigidbody.linearVelocity = Vector2.zero;
                    break;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (_firePoint != null && _hasMaxDistance)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(_firePoint.position, _grappleDistance);
        }
    }
}
