using UnityEngine;

public class TP_Camera : MonoBehaviour
{
    private Player.Movement _player;

    [SerializeField]
    private Vector3 _offset;

    [SerializeField]
    private float _rotationSpeed = 5f;

    [SerializeField]
    private float _minVerticalAngle = -60f;
    [SerializeField]
    private float _maxVerticalAngle = 60f;

    [SerializeField]
    private float _positionDamping = 5f; // Smoothing for camera position

    private float _rotationX = 0f;
    private float _rotationY = 0f;

    private Transform _targetLookAt; // A target transform for LookAt to smooth rotations
    private Vector3 _targetPosition;

    private void Start()
    {
        _player = FindObjectOfType<Player.Movement>();

        // Initialize rotation angles
        Vector3 directionToPlayer = _player.transform.position - transform.position;
        Quaternion initialRotation = Quaternion.LookRotation(directionToPlayer.normalized);
        _rotationY = initialRotation.eulerAngles.y;
        _rotationX = ClampAngle(initialRotation.eulerAngles.x, _minVerticalAngle, _maxVerticalAngle);

        // Create a dummy object for smoothing the LookAt
        _targetLookAt = new GameObject("CameraLookAtTarget").transform;
        _targetLookAt.position = _player.transform.position + Vector3.up;
        _targetLookAt.SetParent(_player.transform); // optional: Make it a child of the player, so it follows player movement, useful if offset should remain relative to the player
    }

    private void FixedUpdate()
    {
        // Обновляем кэшированную позицию и поворот игрока.  Вместо прямого использования player.transform.position
        _targetPosition = _player.transform.position;
        // Если у вас есть поворот, то нужно его сохранить.

    }

    private void LateUpdate() // Возвращаем LateUpdate и применяем сглаживание
    {
        // Handle rotation input (как и раньше)
        // ... rotation input ...

        // Calculate desired position based on rotation and offset
        Quaternion targetRotation = Quaternion.Euler(_rotationX, _rotationY, 0);
        Vector3 desiredPosition = _targetPosition + targetRotation * _offset; // Используем закешированную позицию

        // Smooth position (как и раньше)
        transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * _positionDamping);

        // Smoothly update the LookAt target
        _targetLookAt.position = _targetPosition + Vector3.up;

        transform.LookAt(_player.transform.position + Vector3.up);
    }

    private Vector2 MouseLook()
    {
        return InputManager._instance._inputActions.Camera.Look.ReadValue<Vector2>();
    }

    private float ClampAngle(float angle, float min, float max)
    {
        angle = Mathf.Repeat(angle, 360);
        min = Mathf.Repeat(min, 360);
        max = Mathf.Repeat(max, 360);
        if (angle > 180) angle -= 360;
        if (max > 180) max -= 360;
        if (min > 180) min -= 360;
        return Mathf.Clamp(angle, min, max);
    }
}