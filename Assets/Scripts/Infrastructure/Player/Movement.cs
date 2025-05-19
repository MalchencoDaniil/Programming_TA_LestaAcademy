using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(Rigidbody))]
    public class Movement : MonoBehaviour
    {
        private Rigidbody _rb;
        private PlayerInput _playerInput;

        [Header("References")]
        [SerializeField]
        private Animator _playerAnimator;

        [Header("Movement Settings")]
        [SerializeField]
        private float _movementSpeed = 6;

        [SerializeField]
        private float _rotationSpeed = 15;

        [Header("Jump Settings")]
        [SerializeField]
        private float _jumpForce = 12;

        [SerializeField]
        private float _coyoteTime = 0.2f;

        private float _coyoteTimeCounter;

        [Header("Drag")]
        [SerializeField]
        private float _airDrag = 0.1f;
        [SerializeField]
        private float _groundDrag = 0.3f;

        [Header("Gravity")]
        [SerializeField]
        private float _gravityScale = 3f;

        [Header("Ground Detection")]
        [SerializeField]
        private CapsuleCollider _capsuleCollider;

        [SerializeField]
        private Color _checkColor = Color.green;

        [SerializeField]
        private float _checkRadius = 0.25f;

        [SerializeField]
        private LayerMask _groundLayer;

        private void Awake()
        {
            _playerInput = new PlayerInput();
            _rb = GetComponent<Rigidbody>();
        }

        private void Start()
        {
            _rb.freezeRotation = true;
        }

        private void Update()
        {
            Jump();
            PlayerRotate();
        }

        private void FixedUpdate()
        {
            ApplyDrag();
            LimitVelocity();
            ApplyGravity();
            Move();
        }

        public void Wind(WindArea _currentWindArea, bool _isInWindArea)
        {
            if (_isInWindArea)
            {
                Vector3 _windForce = _currentWindArea.GetWindForce();
                _rb.AddForce(_windForce * 10, ForceMode.Acceleration);
            }
        }

        private void ApplyGravity()
        {
            _rb.AddForce(Vector3.down * _gravityScale * _rb.mass, ForceMode.Acceleration);
        }

        private void Move()
        {
            Vector3 _movementDirection = GetDirection();
            _movementDirection = _movementDirection.normalized;

            _playerAnimator.SetFloat("Speed", _movementDirection.sqrMagnitude);

            Vector3 targetVelocity = _movementDirection * _movementSpeed;

            _rb.velocity = new Vector3(targetVelocity.x, _rb.velocity.y, targetVelocity.z);
        }

        private void LimitVelocity()
        {
            Vector3 _horizontalVelocity = new Vector3(_rb.velocity.x, 0, _rb.velocity.z);

            if (_horizontalVelocity.magnitude > _movementSpeed)
            {
                _horizontalVelocity = _horizontalVelocity.normalized * _movementSpeed;
                _rb.velocity = new Vector3(_horizontalVelocity.x, _rb.velocity.y, _horizontalVelocity.z);
            }
        }

        private void PlayerRotate()
        {
            Vector3 _movementDirection = GetDirection().normalized;

            if (_movementDirection != Vector3.zero)
            {
                transform.forward = Vector3.Slerp(transform.forward, _movementDirection, Time.deltaTime * _rotationSpeed);
            }
        }

        private Vector3 GetDirection()
        {
            Quaternion _cameraDirection = Quaternion.Euler(0, Camera.main.transform.eulerAngles.y, 0);
            Vector3 _input = new Vector3(_playerInput.MovementInput().x, 0, _playerInput.MovementInput().y);

            return (_cameraDirection * _input).normalized;
        }

        private bool IsJumping = false;

        private async void Jump()
        {
            if (IsGrounded())
            {
                _coyoteTimeCounter = _coyoteTime;
            }
            else
            {
                _playerAnimator.SetBool("CanJump", true);
                IsJumping = true;
                _coyoteTimeCounter -= Time.deltaTime;
            }

            if (_coyoteTimeCounter > 0f && _playerInput.CanJump())
            {
                _playerAnimator.SetBool("CanJump", true);

                Vector3 _horizontalVelocity = new Vector3(_rb.velocity.x, 0, _rb.velocity.z);
                _rb.velocity = _horizontalVelocity;

                _rb.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);

                await JumpWaitTime();

                _coyoteTimeCounter = 0f;
            }

            if (IsJumping && IsGrounded())
            {
                _playerAnimator.SetBool("CanJump", false);
                IsJumping = false;
            }
        }

        private async UniTask JumpWaitTime()
        {
            await UniTask.WaitForSeconds(0.3f);
            IsJumping = true;
        }

        private void ApplyDrag()
        {
            float _drag = IsGrounded() ? _groundDrag : _airDrag;

            Vector3 _horizontalVelocity = new Vector3(_rb.velocity.x, 0, _rb.velocity.z);
            Vector3 _dragForce = -_horizontalVelocity.normalized * _drag;

            _rb.AddForce(_dragForce, ForceMode.Acceleration);
        }

        private bool IsGrounded()
        {
            return Physics.CheckSphere(new Vector3(transform.position.x, transform.position.y + (_capsuleCollider.height / 2) * -1, transform.position.z), _checkRadius, _groundLayer);
        }

        private void OnDrawGizmos()
        {
            if (_capsuleCollider != null)
            {
                Gizmos.color = _checkColor;
                Gizmos.DrawSphere(new Vector3(transform.position.x, transform.position.y + (_capsuleCollider.height / 2) * -1, transform.position.z), _checkRadius);
            }
        }
    }
}