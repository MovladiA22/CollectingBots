using System;
using UnityEngine;

public class BaseBuilder : MonoBehaviour
{
    [SerializeField] private Base _basePrefab;

    private CapsuleCollider _baseCollider;
    private Vector3 _buildPosition;
    private Unit _unit;

    public event Action BaseIsBuilt;

    private void Awake()
    {
        if (_basePrefab.TryGetComponent(out CapsuleCollider capsuleCollider))
            _baseCollider = capsuleCollider;
    }

    public void SendBuilder(Unit builder, Transform newBase)
    {
        _buildPosition = newBase.position;
        builder.ReachedToBaseFlag += BuildBase;

        builder.TakeTask(newBase);
        _unit = builder;
    }

    private void BuildBase()
    {
        _unit.ReachedToBaseFlag -= BuildBase;

        float halfDivider = 2f;
        float baseHeight = _baseCollider.height * _basePrefab.transform.localScale.y;

        _buildPosition = new Vector3(_buildPosition.x, _buildPosition.y + baseHeight / halfDivider, _buildPosition.z);
        
        Base resourceBase = Instantiate(_basePrefab, _buildPosition, Quaternion.identity);
        resourceBase.Init(_unit);

        BaseIsBuilt?.Invoke();
    }
}
