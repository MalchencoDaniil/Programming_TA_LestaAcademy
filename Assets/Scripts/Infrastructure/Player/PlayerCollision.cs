using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    [SerializeField]
    private HealthSystem _healthSystem;

    [SerializeField]
    private Player.Movement _player;

    private bool _isInWindArea = false;
    private WindArea _currentWindArea;

    private void FixedUpdate()
    {
        _player.Wind(_currentWindArea, _isInWindArea);
    }

    private void OnTriggerEnter(Collider _other)
    {
        if (_other.gameObject.GetComponent<DamageTrigger>())
        {
            DamageTrigger _damageTrigger = _other.gameObject.GetComponent<DamageTrigger>();
            _damageTrigger.Damage(_healthSystem);
        }

        if (_other.gameObject.GetComponent<JumpTrap>())
        {
            JumpTrap _jumpTrap = _other.gameObject.GetComponent<JumpTrap>();

            Vector3 _horizontalVelocity = new Vector3(_player._rb.velocity.x, 0, _player._rb.velocity.z);
            _player._rb.velocity = _horizontalVelocity;

            _player._rb.AddForce(Vector3.up * _jumpTrap._jumpForceAmount, ForceMode.Impulse);
        }


        if (_other.gameObject.GetComponent<WindArea>())
        {
            WindArea windArea = _other.GetComponent<WindArea>();

            if (windArea != null)
            {
                _isInWindArea = true;
                _currentWindArea = windArea;
            }
        }
    }

    private void OnTriggerStay(Collider _other)
    {
        if (_other.gameObject.GetComponent<PressureTrap>())
        {
            PressureTrap _pressureTrap = _other.gameObject.GetComponent<PressureTrap>();
            _pressureTrap.TrapActivation();
        }
    }

    private void OnTriggerExit(Collider _other)
    {
        if (_other.gameObject.GetComponent<WindArea>())
        {
            WindArea _windArea = _other.GetComponent<WindArea>();

            if (_windArea != null)
            {
                _isInWindArea = false;
                _currentWindArea = null;
            }
        }
    }
}