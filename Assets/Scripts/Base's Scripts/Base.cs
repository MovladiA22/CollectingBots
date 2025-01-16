using System;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class Base : MonoBehaviour
{
    [SerializeField] private ResourceScanner _resourceScanner;
    [SerializeField] private ResourceCollector _resourceCollector;
    [SerializeField] private UnitSpawner _unitSpawner;
    [SerializeField] private FlagInstaller _flagInstaller;
    [SerializeField] private BaseFlag _flag;
    [SerializeField] private BaseBuilder _baseBuilder;
    [SerializeField] private BaseInfoViewer _infoViewer;
    [SerializeField] private ClickProcessor _clickProcessor;
    [SerializeField] private Color _defaultColor;

    private int _resourcesCount;
    private Renderer _renderer;
    private bool _isStaredBuildBase = false;
    private bool _isSelected = false;

    public event Action<string> ResourcesCountChanged;
    public event Action<string> UnitsCountChanged;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
    }

    private void Start()
    {
        _resourceScanner.TurnOn();
    }

    private void OnEnable()
    {
        _resourceScanner.ResourceFound += _resourceCollector.AddResourceToList;
        _resourceCollector.ResourceIsGot += ProcessResource;
        _clickProcessor.OnBaseAndLandClicked += RemoveSelectivity;
        _baseBuilder.BaseIsBuilt += ReturnFlag;
    }

    private void OnDisable()
    {
        _resourceScanner.ResourceFound -= _resourceCollector.AddResourceToList;
        _resourceCollector.ResourceIsGot -= ProcessResource;
        _clickProcessor.OnBaseAndLandClicked -= RemoveSelectivity;
        _baseBuilder.BaseIsBuilt -= ReturnFlag;
    }

    public void Init(Unit unit)
    {
        _resourceCollector.Init();
        _resourceCollector.TakeUnit(unit);
        unit.AssighBasePosition(transform);
        _resourceScanner.TurnOff();
        _resourceScanner.TurnOn();

        RemoveSelectivity();
    }

    public void Select()
    {
        _isSelected = true;
        _renderer.material.color = Color.blue;

        _infoViewer.ToggleTextVisibility(_isSelected, this);

        UnitsCountChanged?.Invoke(_resourceCollector.UnitsCount.ToString());
        ResourcesCountChanged?.Invoke(_resourcesCount.ToString());

        if (_isStaredBuildBase == false)
            _flagInstaller.ToggleFlagActivation(_isSelected, _flag);
    }

    private void RemoveSelectivity()
    {
        _isSelected = false;
        _renderer.material.color = _defaultColor;

        _infoViewer.ToggleTextVisibility(_isSelected, this);

        if (_isStaredBuildBase == false)
            _flagInstaller.ToggleFlagActivation(_isSelected, _flag);
    }

    private void ProcessResource(Resource resource)
    {
        resource.InvokeEventProcessed();

        _resourcesCount++;
        ResourcesCountChanged?.Invoke(_resourcesCount.ToString());

        if (_flag.IsInstalled == false && TryCreateUnit(out Unit unit))
        {
            _resourceCollector.TakeUnit(unit);
            UnitsCountChanged?.Invoke(_resourceCollector.UnitsCount.ToString());
        }
        else if (_flag.IsInstalled && TryStartBuildNewBase())
        {
            if (_resourceCollector.TryGiveAwayUnit(out Unit builder))
            {
                _isStaredBuildBase = true;
                _baseBuilder.SendBuilder(builder, _flag.transform);
                UnitsCountChanged?.Invoke(_resourceCollector.UnitsCount.ToString());
            }
        }
    }

    private bool TryCreateUnit(out Unit unit)
    {
        unit = null;
        int unitPrice = 3;

        if (_resourcesCount >= unitPrice)
        {
            _resourcesCount -= unitPrice;
            ResourcesCountChanged?.Invoke(_resourcesCount.ToString());

            unit = _unitSpawner.Create(transform);

            return true;
        }

        return false;
    }

    private bool TryStartBuildNewBase()
    {
        int basePrice = 5;

        if (_resourcesCount >= basePrice && _isStaredBuildBase == false)
        {
            _resourcesCount -= basePrice;
            ResourcesCountChanged?.Invoke(_resourcesCount.ToString());

            return true;
        }

        return false;
    }

    private void ReturnFlag()
    {
        _flag.Remove();
        _flag.transform.position = transform.position;
        _flag.gameObject.SetActive(false);
        _isStaredBuildBase = false;
    }
}
