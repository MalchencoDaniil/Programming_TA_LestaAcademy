using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    [SerializeField]
    private HealthSystem _healthSystem;

    private void OnTriggerEnter(Collider _other)
    {
        if (_other.gameObject.GetComponent<DamageTrigger>())
        {
            DamageTrigger _damageTrigger = _other.gameObject.GetComponent<DamageTrigger>();
            _damageTrigger.Damage(_healthSystem);
        }
    }
}