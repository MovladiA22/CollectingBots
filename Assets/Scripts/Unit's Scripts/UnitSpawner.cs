using UnityEngine;

public class UnitSpawner : MonoBehaviour
{
    [SerializeField] private Unit _prefab;

    public Unit Create(Transform spawnPosition)
    {
        Unit copy = Instantiate(_prefab, spawnPosition.position, Quaternion.identity);
        copy.AssighBasePosition(spawnPosition);

        return copy;
    }
}
