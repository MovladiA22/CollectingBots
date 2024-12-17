using System;
using UnityEngine;

public class SpawnZone : MonoBehaviour
{
    public event Action<Transform> ResourceIsLost;

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<Resource>(out Resource resource))
            ResourceIsLost?.Invoke(transform);
    }
}
