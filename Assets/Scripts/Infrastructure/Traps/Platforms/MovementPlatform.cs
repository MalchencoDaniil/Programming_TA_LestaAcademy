using UnityEngine;

public class MovementPlatform : MonoBehaviour
{
    private int _currentPointIndex = 0;
    private float _distanceThreshold = 0.1f;

    private GameObject _player;
    private Rigidbody _playerRb;

    private Vector3 _lastPlatformPosition;

    [Header("Movement Points")]
    [SerializeField] private WaypointManager _path;
    [SerializeField] private float _speed = 2f;

    [SerializeField] private float maxVelocity = 20;

    private void Start()
    {
        transform.position = _path._pathPoints[_currentPointIndex].position;
    }

    private void FixedUpdate()
    {
        MovePlatform();

        Vector3 _platformVelocity = (transform.position - _lastPlatformPosition) / Time.fixedDeltaTime;

        if (_player != null && _playerRb != null)
        {
            _playerRb.velocity += _platformVelocity;

            Vector3 _horizontalVelocity = new Vector3(_playerRb.velocity.x, 0, _playerRb.velocity.z);

            if (_horizontalVelocity.magnitude > maxVelocity)
            {
                _horizontalVelocity = _horizontalVelocity.normalized * maxVelocity;
                _playerRb.velocity = new Vector3(_horizontalVelocity.x, _playerRb.velocity.y, _horizontalVelocity.z);
            }
        }

        _lastPlatformPosition = transform.position;
    }

    private void MovePlatform()
    {
        Vector3 _targetPosition = _path._pathPoints[_currentPointIndex].position;
        transform.position = Vector3.MoveTowards(transform.position, _targetPosition, _speed * Time.fixedDeltaTime);

        if (Vector3.Distance(transform.position, _targetPosition) < _distanceThreshold)
        {
            _currentPointIndex = (_currentPointIndex + 1) % _path._pathPoints.Count;
        }
    }

    private void OnCollisionEnter(Collision _collision)
    {
        if (_collision.gameObject.GetComponent<Player.Movement>())
        {
            _player = _collision.gameObject;
            _playerRb = _player.GetComponent<Rigidbody>();

            if (_playerRb != null)
                _lastPlatformPosition = transform.position;
        }
    }

    private void OnCollisionExit(Collision _collision)
    {
        if (_collision.gameObject.GetComponent<Player.Movement>())
        {
            _player = null;

            if (_playerRb != null)
                _playerRb = null;
        }
    }
}