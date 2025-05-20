using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

public class PressureTrap : MonoBehaviour
{
    private bool _canActivated;
    private bool _playerInZone;

    [Header("References")]
    [SerializeField]
    private Animator _trapAnimator;

    [Header("Mesh Settings")]
    [SerializeField]
    private MeshRenderer _mesh;

    [SerializeField]
    private Material _baseMaterial;

    [SerializeField]
    private Material _activatedMaterial;

    [Header("Trap Settings")]
    [SerializeField, Range(0, 5)]
    private float _attackTime = 0.5f;

    [SerializeField]
    private float _reloadTime = 5;

    [Header("Events")]
    [SerializeField]
    private UnityEvent _hitEvent;

    private void Start()
    {
        _mesh.material = _baseMaterial;
    }

    public async void TrapActivation()
    {
        if (!_canActivated)
            await PressureTrapLogic();
    }

    private async UniTask PressureTrapLogic()
    {
        _canActivated = true;

        _mesh.material = _activatedMaterial;

        await UniTask.WaitForSeconds(_attackTime);

        if (_playerInZone)
            _hitEvent.Invoke();

        _trapAnimator.SetTrigger("TakeHit");

        _mesh.material = _baseMaterial;

        await UniTask.WaitForSeconds(_reloadTime);

        _canActivated = false;
    }

    private void OnTriggerEnter(Collider _other)
    {
        if (_other.gameObject.GetComponent<Player.Movement>())
        {
            _playerInZone = true;
        }
    }

    private void OnTriggerExit(Collider _other)
    {
        if (_other.gameObject.GetComponent<Player.Movement>())
        {
            _playerInZone = false;
        }
    }
}