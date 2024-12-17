using System;
using UnityEngine;

public class Resource : MonoBehaviour
{
    public event Action<Resource> Processed;

    public bool Captured {  get; private set; }

    private void OnEnable()
    {
        Captured = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Unit>(out Unit unit) && Captured == false)
            Captured = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<Base>(out Base resourseBase) && Captured)
        {
            Captured = false;
            Processed?.Invoke(this);
        }
    }
}
