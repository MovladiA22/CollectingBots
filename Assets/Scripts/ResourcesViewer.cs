using TMPro;
using UnityEngine;

public class ResourcesViewer : MonoBehaviour
{
    private const string ResourcesName = "Ресурсы: ";
    private const string StartValue = "0";

    [SerializeField] private Base _resourceBase;
    [SerializeField] private TextMeshProUGUI _textMeshPro;

    private void Start()
    {
        Renderer(StartValue);
    }

    private void OnEnable()
    {
        _resourceBase.ResourcesCountChanged += Renderer;
    }

    private void OnDisable()
    {
        _resourceBase.ResourcesCountChanged -= Renderer;
    }

    public void Renderer(string text)
    {
        _textMeshPro.text = ResourcesName + text;
    }
}
