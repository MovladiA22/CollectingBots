using System;
using UnityEngine;

public class Base : MonoBehaviour
{
    [SerializeField] private ResourceScanner _resourceScanner;
    [SerializeField] private ResourceCollector _resourceCollector;

    private int resourcesCount;

    public event Action<string> ResourcesCountChanged;

    private void Start()
    {
        _resourceScanner.TurnOn();
    }

    private void OnEnable()
    {
        _resourceScanner.ResourceFound += _resourceCollector.AddResourceToList;
        _resourceCollector.ResourceIsGot += ProcessResource;
    }

    private void OnDisable()
    {
        _resourceScanner.ResourceFound -= _resourceCollector.AddResourceToList;
        _resourceCollector.ResourceIsGot -= ProcessResource;
    }

    private void ProcessResource(Resource resource)
    {
        resource.InvokeEvent();

        resourcesCount++;
        ResourcesCountChanged?.Invoke(resourcesCount.ToString());
    }
}
