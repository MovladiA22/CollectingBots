using UnityEngine;

public class BaseFlag : MonoBehaviour
{
    public bool IsInstalled { get; private set; } = false;

    public void Install() =>
        IsInstalled = true;

    public void Remove() =>
        IsInstalled = false;
}
