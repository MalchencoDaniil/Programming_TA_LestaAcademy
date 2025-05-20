using Cysharp.Threading.Tasks;
using System.Collections;
using UnityEngine;

public class WindArea : MonoBehaviour
{
    private bool _workState = true;

    private Vector3 _windDirection;

    [Header("References")]
    [SerializeField]
    private WindZone _windZone;

    [Header("Wind Settings")]
    [SerializeField] private float _windForce = 10f;
    [SerializeField] private float _changeDirectionInterval = 2f;

    private async void Start()
    {
        await ChangeWindDirection();
    }

    private async UniTask ChangeWindDirection()
    {
        while (_workState)
        {
            _windDirection = Quaternion.Euler(0, Random.Range(0f, 360f), 0) * Vector3.forward;

            _windZone.transform.rotation = Quaternion.LookRotation(_windDirection);

            await UniTask.WaitForSeconds(_changeDirectionInterval);
        }
    }

    public Vector3 GetWindForce()
    {
        return _windDirection * _windForce;
    }

    private void OnDestroy()
    {
        _workState = false;
    }
}