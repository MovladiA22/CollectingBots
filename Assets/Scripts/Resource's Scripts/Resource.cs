using System;
using UnityEngine;

public class Resource : MonoBehaviour
{
    public event Action<Resource> Processed;

    public bool IsExtracted { get; private set; } = false;

    private void OnEnable()
    {
        IsExtracted = false;
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<Land>(out Land land))
        {
            IsExtracted = true;
        }
    }

    public void InvokeEventProcessed() =>
        Processed?.Invoke(this);
}
