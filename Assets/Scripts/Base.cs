using System;
using UnityEngine;

public class Base : MonoBehaviour
{
    [SerializeField] private ResourceScanner _resourceScanner;
    [SerializeField] private ResourceCollector _resourceCollector;

    private int resourceCount;

    public event Action<string> ResourcesCountChanged;

    private void Start()
    {
        _resourceScanner.TurnOn();
    }

    private void OnEnable()
    {
        _resourceScanner.ResourceFound += _resourceCollector.PickUpResource;
        _resourceCollector.ResourceIsGot += ProcessResource;
    }

    private void OnDisable()
    {
        _resourceScanner.ResourceFound -= _resourceCollector.PickUpResource;
        _resourceCollector.ResourceIsGot -= ProcessResource;
    }

    private void ProcessResource()
    {
        resourceCount++;
        ResourcesCountChanged?.Invoke(resourceCount.ToString());
    }
}
