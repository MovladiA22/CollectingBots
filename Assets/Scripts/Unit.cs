using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] private UnitMover _mover;
    [SerializeField] private Transform _base;

    private bool _isLoaded = false;
    private Vector3 _startPosition;

    public bool IsFree { get; private set; } = true;

    private void Start()
    {
        _startPosition = transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Resource resource) && _isLoaded == false)
        {
            _isLoaded = true;
            resource.Processed += ReturnBack;

            resource.transform.parent = transform;
            _mover.MoveToTarget(_base.position);
        }
    }

    public void TakeTask(Vector3 targetPosition)
    {
        IsFree = false;
        _mover.MoveToTarget(targetPosition);
    }

    private void ReturnBack(Resource resource)
    {
        resource.Processed -= ReturnBack;

        _mover.MoveToTarget(_startPosition);
        _isLoaded = false;
        IsFree = true;
    }
}
