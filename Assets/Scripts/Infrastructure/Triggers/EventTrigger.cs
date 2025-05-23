using UnityEngine;
using UnityEngine.Events;

public class EventTrigger : MonoBehaviour
{
    [SerializeField]
    private UnityEvent _triggerEvent;

    private void OnTriggerEnter(Collider _other)
    {
        if (_other.gameObject.GetComponent<Player.Movement>())
        {
            _triggerEvent.Invoke();
        }
    }
}