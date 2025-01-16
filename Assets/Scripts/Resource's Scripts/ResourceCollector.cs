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
        unit = null;

        if (_collectors.Count > 1)
        {
            unit = _collectors[0];
            _collectors[0].ResourceGaveAway -= AcceptResource;
            _collectors.RemoveAt(0);

            return true;
        }

        return false;
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
            PickUpResource(_resources[0]);

            yield return wait;
        }
    }

    private void PickUpResource(Resource resource)
    {
        if (TryGetFreeUnit(out Unit unit))
        {
            _resources.Remove(resource);
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
