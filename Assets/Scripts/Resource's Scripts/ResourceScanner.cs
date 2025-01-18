using System;
using System.Collections;
using UnityEditor.PackageManager;
using UnityEngine;

public class ResourceScanner : MonoBehaviour
{
    [SerializeField] private float _scanSpeed;
    [SerializeField] private float _scanRadius;
    [SerializeField] private float _scanIntervalTime;

    private bool _isTurnedOn = false;
    private YieldInstruction _wait;

    public event Action<Resource> ResourceFound;

    private void Awake()
    {
        _wait = new WaitForFixedUpdate();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Resource resource))
            if (resource.IsExtracted == false)
                ResourceFound?.Invoke(resource);
    }

    public void TurnOn()
    {
        if (_isTurnedOn == false)
        {
            _isTurnedOn = true;
            StartCoroutine(ActivateRegularScanner());
        }
    }

    public void TurnOff()
    {
        if (_isTurnedOn)
            _isTurnedOn = false;
    }

    private IEnumerator ActivateRegularScanner()
    {
        var wait = new WaitForSeconds(_scanIntervalTime);

        while (_isTurnedOn)
        {
            yield return wait;
            
            Activate();
        }
    }

    private void Activate()
    {
        StartCoroutine(Scanning());
    }

    private IEnumerator Scanning()
    {
        float newRadius;
        Vector3 newScale;
        Vector3 defaultScale = transform.localScale;

        while (transform.localScale.x != _scanRadius)
        {
            newRadius = Mathf.MoveTowards(transform.localScale.x, _scanRadius, _scanSpeed);
            newScale = new Vector3(newRadius, transform.localScale.y, newRadius);
            transform.localScale = newScale;

            yield return _wait;
        }

        transform.localScale = defaultScale;
    }
}
