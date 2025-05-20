using UnityEngine;
using UnityEngine.Events;

public class HealthSystem : MonoBehaviour, IDamageble
{
    private float _maxHealth;

    [SerializeField]
    private float _health = 100;

    [Header("Events")]
    [SerializeField]
    private UnityEvent OnDeath;

    public UnityEvent OnTakeDamage;

    private void Start()
    {
        _maxHealth = _health;
    }

    public float CurrentHealth => _health;

    public void TakeDamage(float _damage)
    {
        _health -= _damage;
        _health = CheckHealth(_health);

        OnTakeDamage?.Invoke();
    }

    private float CheckHealth(float _health)
    {
        if (_health <= 0)
        {
            OnDeath.Invoke();
            return 0;
        }

        if (_health >= _maxHealth)
            return _maxHealth;

        return _health;
    }
}