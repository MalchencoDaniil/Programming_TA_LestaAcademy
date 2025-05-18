using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    [SerializeField]
    private HealthSystem _healthSystem;

    [SerializeField]
    private Slider _healthSlider;

    private void Start()
    {
        _healthSlider.maxValue = _healthSystem.CurrentHealth;

        _healthSystem.OnTakeDamage.AddListener(CanvasUpateHealth);
        CanvasUpateHealth();
    }

    private void OnDestroy()
    {
        if (_healthSystem != null)
        {
            _healthSystem.OnTakeDamage.RemoveListener(CanvasUpateHealth);
        }
    }

    public void CanvasUpateHealth()
    {
        _healthSlider.value = _healthSystem.CurrentHealth;
    }
}