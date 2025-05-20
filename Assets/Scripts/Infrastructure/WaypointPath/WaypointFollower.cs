using UnityEngine;

public class WaypointFollower : MonoBehaviour
{
    private Vector3 _startPosition, _endPosition, _startTangent;
    private Vector3 _endTangent;

    private int _currentWaypointIndex = 0;
    private float _t = 0f;
    private float _pathLength = 0f;

    private Quaternion _targetRotation;

    [SerializeField] 
    private WaypointManager _waypointManager;

    [SerializeField]
    private float _movementSpeed = 5f;

    [SerializeField]
    private float _rotationSpeed = 5f;

    private void Start()
    {
        transform.position = _waypointManager._pathPoints[0].position;
        InitializePathSegment();
    }

    private void Update()
    {
        if (_waypointManager == null || _waypointManager._pathPoints.Count < 2) return;

        _t += Time.deltaTime * _movementSpeed / _pathLength;
        Vector3 _nextPosition = CalculateBezierPoint(_t, _startPosition, _endPosition, _startTangent, _endTangent);

        Vector3 _direction = _nextPosition - transform.position;

        if (_direction != Vector3.zero)
        {
            _targetRotation = Quaternion.LookRotation(_direction);
        }

        transform.rotation = Quaternion.Slerp(transform.rotation, _targetRotation, Time.deltaTime * _rotationSpeed);

        transform.position = _nextPosition;

        if (_t >= 1f)
        {
            _currentWaypointIndex = (_currentWaypointIndex + 1) % _waypointManager._pathPoints.Count;
            InitializePathSegment();
            _t = 0f;
        }
    }


    private void InitializePathSegment()
    {
        Transform _startPoint = _waypointManager._pathPoints[_currentWaypointIndex];
        Transform _endPoint = _waypointManager._pathPoints[(_currentWaypointIndex + 1) % _waypointManager._pathPoints.Count];

        _startPosition = _startPoint.position;
        _endPosition = _endPoint.position;

        _startTangent = _startPosition + _startPoint.forward * Vector3.Distance(_startPosition, _endPosition) * _waypointManager._handleSize;
        _endTangent = _endPosition - _endPoint.forward * Vector3.Distance(_startPosition, _endPosition) * _waypointManager._handleSize;

        _pathLength = CalculateBezierLength(_startPosition, _endPosition, _startTangent, _endTangent);

        Vector3 _direction = _endPosition - _startPosition;

        if (_direction != Vector3.zero)
        {
            _targetRotation = Quaternion.LookRotation(_direction);
        }
    }

    private float CalculateBezierLength(Vector3 p0, Vector3 p3, Vector3 p1, Vector3 p2)
    {
        float _length = 0f;
        Vector3 _previousPosition = p0;
        for (int i = 1; i <= 20; i++)
        {
            float t = i / 20f;
            Vector3 _currentPosition = CalculateBezierPoint(t, p0, p3, p1, p2);
            _length += Vector3.Distance(_currentPosition, _previousPosition);
        }
        return _length;
    }


    private Vector3 CalculateBezierPoint(float t, Vector3 p0, Vector3 p3, Vector3 p1, Vector3 p2)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        float uuu = uu * u;
        float ttt = tt * t;

        Vector3 p = uuu * p0;
        p += 3 * uu * t * p1;
        p += 3 * u * tt * p2;
        p += ttt * p3;

        return p;
    }
}