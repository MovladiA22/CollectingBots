using TMPro;
using UnityEngine;

public class BaseInfoViewer : MonoBehaviour
{
    [SerializeField] private Base _resourceBase;
    [SerializeField] private TextMeshProUGUI _numberOfResources;
    [SerializeField] private TextMeshProUGUI _numberOfUnits;

    private string _beginningOfText;

    public void ToggleTextVisibility(bool isVisible, Base sourceOfInformation)
    {
        if (isVisible)
        {
            sourceOfInformation.ResourcesCountChanged += RendererResources;
            sourceOfInformation.UnitsCountChanged += RendererUnits;
        }
        else
        {
            sourceOfInformation.ResourcesCountChanged -= RendererResources;
            sourceOfInformation.UnitsCountChanged -= RendererUnits;
        }

        _numberOfResources.enabled = isVisible;
        _numberOfUnits.enabled = isVisible;
    }

    private void RendererResources(string text)
    {
        _beginningOfText = "Ресурсы: ";
        _numberOfResources.text = _beginningOfText + text;
    }

    private void RendererUnits(string text)
    {
        _beginningOfText = "Юниты: ";
        _numberOfUnits.text = _beginningOfText + text;
    }
}
