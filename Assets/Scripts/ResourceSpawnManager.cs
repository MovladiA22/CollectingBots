using System.Collections;
using UnityEngine;

public class ResourceSpawnManager : MonoBehaviour
{
    [SerializeField] private ResourceSpawner _spawner;
    [SerializeField] private SpawnZone[] _spawnZones;
    [SerializeField] private float _minSpawnDelay;
    [SerializeField] private float _maxSpawnDelay;

    private void Start()
    {
        foreach (var spawnZone in _spawnZones)
        {
            Spawn(spawnZone.transform);
        }
    }

    private void OnEnable()
    {
        foreach (var spawnZone in _spawnZones)
        {
            spawnZone.ResourceIsLost += Spawn;
        }
    }

    private void OnDisable()
    {
        foreach (var spawnZone in _spawnZones)
        {
            spawnZone.ResourceIsLost -= Spawn;
        }
    }

    private void Spawn(Transform spawnZone)
    {
        StartCoroutine(SpawnWithDelay(spawnZone));
    }

    private IEnumerator SpawnWithDelay(Transform spawnZone)
    {
        var wait = new WaitForSeconds(Random.Range(_minSpawnDelay, _maxSpawnDelay + 1));

        yield return wait;

        _spawner.GetObj(spawnZone.position);
    }
}
