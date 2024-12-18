using System;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] private UnitMover _mover;
    [SerializeField] private Transform _base;

    private bool _isLoaded = false;
    private Vector3 _startPosition;
    private Resource _resource;

    public event Action<Resource> ResourceGaveAway;

    public bool IsFree { get; private set; } = true;

    private void Start()
    {
        _startPosition = transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Resource resource) && _isLoaded == false)
            TakeResource(resource);

        if (other.TryGetComponent<BaseZone>(out BaseZone baseZone) && _isLoaded)
            GiveAwayResource(_resource);
    }

    public void TakeTask(Vector3 targetPosition)
    {
        IsFree = false;
        _mover.MoveToTarget(targetPosition);
    }

    private void TakeResource(Resource resource)
    {
        _isLoaded = true;
        _resource = resource;
        resource.transform.parent = transform;
        _mover.MoveToTarget(_base.position);
    }

    private void GiveAwayResource(Resource resource)
    {
        ResourceGaveAway?.Invoke(resource);

        _resource = null;
        _isLoaded = false;
        IsFree = true;
        _mover.MoveToTarget(_startPosition);
    }
}
