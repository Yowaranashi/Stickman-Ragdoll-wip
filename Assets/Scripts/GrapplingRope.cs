using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class GrapplingRope : MonoBehaviour
{
    [SerializeField] private GrapplingGun _grapplingGun;
    [Header("General Settings:")]
    [SerializeField] private int _percision = 40;
    [Range(0, 20)][SerializeField] private float _straightenLineSpeed = 5;
    [Header("Rope Animation Settings:")]
    [SerializeField] private AnimationCurve _ropeAnimationCurve;
    [Range(0.01f, 4)][SerializeField] private float _startWaveSize = 2;
    [Header("Rope Progression:")]
    [SerializeField] private AnimationCurve _ropeProgressionCurve;
    [SerializeField][Range(1, 50)] private float _ropeProgressionSpeed = 1;
    [HideInInspector] public bool IsGrappling = true;
    private LineRenderer _lineRenderer;
    private float _moveTime = 0;
    private bool _strightLine = true;
    private float _waveSize = 0;
    private void OnEnable()
    {
        if(_lineRenderer == null)
            _lineRenderer = GetComponent<LineRenderer>();
        _moveTime = 0;
        _lineRenderer.positionCount = _percision;
        _waveSize = _startWaveSize;
        _strightLine = false;
        LinePointsToFirePoint();
        _lineRenderer.enabled = true;
    }
    private void OnDisable()
    {
        _lineRenderer.enabled = false;
        IsGrappling = false;
    }
    private void LinePointsToFirePoint()
    {
        for (int i = 0; i < _percision; i++)
        {
            _lineRenderer.SetPosition(i, _grapplingGun.FirePoint.position);
        }
    }
    private void Update()
    {
        _moveTime += Time.deltaTime;
        DrawRope();
    }
    private void DrawRope()
    {
        if (!_strightLine)
        {
            //!
            if (Mathf.Abs(_lineRenderer.GetPosition(_percision - 1).x - _grapplingGun.GrapplePoint.x) > 0.25f)
            {
                _strightLine = true;
            }
            else
            {
                DrawRopeWaves();
            }
        }
        else
        {
            if (!IsGrappling && _grapplingGun.HasGrappleObject)
            {
                _grapplingGun.Grapple();
                IsGrappling = true;
            }
            if (_waveSize > 0)
            {
                _waveSize -= Time.deltaTime * _straightenLineSpeed;
                DrawRopeWaves();
            }
            else
            {
                _waveSize = 0;

                if (_lineRenderer.positionCount != 2) { _lineRenderer.positionCount = 2; }
                DrawRopeNoWaves();
                if (!_grapplingGun.HasGrappleObject)
                    enabled = false;
            }
        }
    }
    private void DrawRopeWaves()
    {
        for (int i = 0; i < _percision; i++)
        {
            float delta = i / (_percision - 1f);
            Vector2 offset = Vector2.Perpendicular(_grapplingGun.GrappleDistanceVector).normalized * _ropeAnimationCurve.Evaluate(delta) * _waveSize;
            Vector2 targetPosition = Vector2.Lerp(_grapplingGun.FirePoint.position, _grapplingGun.GrapplePoint, delta) + offset;
            Vector2 currentPosition = Vector2.Lerp(_grapplingGun.FirePoint.position, targetPosition, _ropeProgressionCurve.Evaluate(_moveTime) * _ropeProgressionSpeed);
            _lineRenderer.SetPosition(i, currentPosition);
        }
    }
    private void DrawRopeNoWaves()
    {
        _lineRenderer.SetPosition(0, _grapplingGun.FirePoint.position);
        _lineRenderer.SetPosition(1, _grapplingGun.GrapplePoint);
    }
}
