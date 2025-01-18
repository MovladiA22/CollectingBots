using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceCollector : MonoBehaviour
{
    [SerializeField] private List<Unit> _collectors;

    private List<Resource> _resources = new List<Resource>();
    private Coroutine _coroutine;

    public event Action<Resource> ResourceIsGot;

    public int UnitsCount => _collectors.Count;

    private void OnEnable()
    {
        foreach (var collector in _collectors)
            collector.ResourceGaveAway += AcceptResource;
    }

    private void OnDisable()
    {
        foreach (var collector in _collectors)
            collector.ResourceGaveAway -= AcceptResource;
    }

    public void Init()
    {
        foreach (var collector in _collectors)
            collector.ResourceGaveAway -= AcceptResource;

        _collectors.Clear();
        _resources.Clear();
    }

    public void TakeUnit(Unit unit)
    {
        _collectors.Add(unit);
        unit.ResourceGaveAway += AcceptResource;
    }

    public bool TryGiveAwayUnit(out Unit unit)
    {
        unit = _collectors[0];

        if (_collectors.Count <= 1)
            return false;

        if (TryGetFreeUnit(out Unit freeUnit))
            unit = freeUnit;
        else if (unit.transform.childCount > 0)
            if (unit.transform.GetChild(0).TryGetComponent(out Resource resource))
                resource.InvokeEventProcessed();

        unit.ResourceGaveAway -= AcceptResource;
        _collectors.Remove(unit);

        return true;
    }

    public void AddResourceToList(Resource resource)
    {
        if (_resources.Contains(resource) == false)
        {
            _resources.Add(resource);

            if (_coroutine != null)
                StopCoroutine(_coroutine);

            _coroutine = StartCoroutine(CollectingResources());
        }
    }

    private IEnumerator CollectingResources()
    {
        float delay = 0.5f;
        var wait = new WaitForSeconds(delay);

        while (_resources.Count > 0)
        {
            if (_resources[0].IsWaitingForCollector)
                _resources.RemoveAt(0);

            if (_resources.Count > 0)
                PickUpResource(_resources[0]);

            yield return wait;
        }
    }

    private void PickUpResource(Resource resource)
    {
        if (TryGetFreeUnit(out Unit unit))
        {
            _resources.Remove(resource);
            resource.MarkAsTargetForCollector();
            unit.TakeTask(resource.transform);
        }
    }

    private bool TryGetFreeUnit(out Unit unit)
    {
        foreach (Unit collector in _collectors)
        {
            if (collector.IsFree)
            {
                unit = collector;

                return true;
            }
        }

        unit = null;
        return false;
    }

    private void AcceptResource(Resource resource)
    {
        ResourceIsGot?.Invoke(resource);
    }
}
