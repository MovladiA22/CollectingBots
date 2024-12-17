using UnityEngine;
using UnityEngine.Pool;

public class ResourceSpawner : MonoBehaviour
{
    [SerializeField] private Resource _prefab;
    [SerializeField] private int _poolCapacity;
    [SerializeField] private int _maxSize;

    private Vector3 _spawnPosition;
    private ObjectPool<Resource> _pool;

    private void Awake()
    {
        _pool = new ObjectPool<Resource>
            (createFunc: () => CreateObj(),
            actionOnGet: (resource) => ActivateObj(resource),
            actionOnRelease: (resource) => DeactivateObj(resource),
            actionOnDestroy: (resource) => DestroyObj(resource),
            collectionCheck: false,
            defaultCapacity: _poolCapacity,
            maxSize: _maxSize);
    }

    public void GetObj(Vector3 spawnPosition)
    {
        _spawnPosition = spawnPosition;
        _pool.Get();
    }

    public void ReleaseObj(Resource resource)
    {
        _pool.Release(resource);
    }

    private Resource CreateObj()
    {
        var copy = Instantiate(_prefab, _spawnPosition, Quaternion.identity);
        copy.Processed += ReleaseObj;

        return copy;
    }

    private void ActivateObj(Resource resource)
    {
        resource.transform.position = _spawnPosition;
        resource.transform.rotation = Quaternion.identity;
        resource.gameObject.SetActive(true);

        resource.Processed += ReleaseObj;
    }

    private void DeactivateObj(Resource resource)
    {
        resource.transform.parent = null;
        resource.Processed -= ReleaseObj;

        resource.gameObject.SetActive(false);
    }

    private void DestroyObj(Resource resource)
    {
        Destroy(resource.gameObject);
    }
}
