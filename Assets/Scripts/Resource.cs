using System;
using UnityEngine;

public class Resource : MonoBehaviour
{
    public event Action<Resource> Processed;

    public void InvokeEvent() =>
        Processed?.Invoke(this);
}
