using UnityEngine;

public class DamageTrigger : MonoBehaviour
{
    [SerializeField]
    private float _damageAmount;

    public void Damage(HealthSystem _healthSystem)
    {
        _healthSystem.TakeDamage(_damageAmount);
    }
}