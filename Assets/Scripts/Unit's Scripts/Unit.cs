using System;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] private UnitMover _mover;
    [SerializeField] private Transform _base;

    private bool _isLoaded = false;
    private Resource _resource;
    private Transform _target;

    public event Action<Resource> ResourceGaveAway;
    public event Action ReachedToBaseFlag;

    public bool IsFree { get; private set; } = true;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Resource resource) && _isLoaded == false)
        {
            if (resource.IsExtracted == false && _target.transform == resource.transform)
                TakeResource(resource);
        }
        else if (other.TryGetComponent(out BaseZone baseZone) && _isLoaded)
        {
            if (_base == baseZone.transform.parent.transform)
                GiveAwayResource();
        }
        else if (other.TryGetComponent(out BaseFlag baseFlag))
        {
            if (_target.transform == baseFlag.transform)
            {
                ReachedToBaseFlag?.Invoke();
                _mover.MoveToTarget(_base.position);
            }
        }
    }

    private void OnEnable()
    {
        _mover.ReachedToTarget += BecomeFree;
    }

    private void OnDisable()
    {
        _mover.ReachedToTarget -= BecomeFree;
    }

    public void AssighBasePosition(Transform basePosition)
    {
        _base = basePosition;
    }

    public void TakeTask(Transform target)
    {
        _target = target;
        IsFree = false;
        _mover.MoveToTarget(target.position);
    }

    private void TakeResource(Resource resource)
    {
        _isLoaded = true;
        _resource = resource;
        resource.transform.parent = transform;
        _mover.MoveToTarget(_base.position);
    }

    private void GiveAwayResource()
    {
        ResourceGaveAway?.Invoke(_resource);

        _resource = null;
        _isLoaded = false;
        IsFree = true;
    }

    private void BecomeFree()
    {
        IsFree = true;
    }
}
