using System;
using UnityEngine;

public class Land : MonoBehaviour
{
    public event Action<Vector3> Clicked;

    public void InvokeClickedEvent(Vector3 clickPosition)
    {
        Clicked?.Invoke(clickPosition);
    }
}
