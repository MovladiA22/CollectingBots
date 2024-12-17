using System;
using UnityEngine;

public class ResourceCollector : MonoBehaviour
{
    [SerializeField] private Unit[] _collectors;

    public event Action ResourceIsGot;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<Resource>(out Resource resource))
            ResourceIsGot?.Invoke();
    }

    public void PickUpResource(Vector3 resourcePosition)
    {
        if (TryGetFreeUnit(out Unit unit))
            unit.TakeTask(resourcePosition);
        else
            Debug.Log("Нет свободных юнитов");

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
