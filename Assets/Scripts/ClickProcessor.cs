using System;
using UnityEngine;

public class ClickProcessor : MonoBehaviour
{
    [SerializeField] private Camera _camera;

    public event Action OnBaseAndLandClicked;

    private void Update()
    {
        ProcessClick();
    }

    private void ProcessClick()
    {
        int numberForMouseButton = 0;

        if (Input.GetMouseButtonDown(numberForMouseButton))
        {
            float maxDistance = 300;
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, maxDistance))
            {
                if (hit.collider.TryGetComponent(out Base resourceBase))
                {
                    OnBaseAndLandClicked?.Invoke();
                    resourceBase.Select();
                }
                else if (hit.collider.TryGetComponent(out Land land))
                {
                    land.InvokeClickedEvent(hit.point);
                    OnBaseAndLandClicked?.Invoke();
                }
            }
        }
    }
}
