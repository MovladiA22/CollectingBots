using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceCollector : MonoBehaviour
{
    [SerializeField] private Unit[] _collectors;

    private List<Resource> _resources = new List<Resource>();
    private Coroutine _coroutine;

    public event Action<Resource> ResourceIsGot;

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
            PickUpResource(_resources[_resources.Count - 1]);

            yield return wait;
        }
    }

    private void PickUpResource(Resource resource)
    {
        if (TryGetFreeUnit(out Unit unit))
        {
            _resources.Remove(resource);
            unit.TakeTask(resource);
        }
    }

    private void AcceptResource(Resource resource)
    {
        ResourceIsGot?.Invoke(resource);
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
}
