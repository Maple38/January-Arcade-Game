using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Bounds bounds;
    [SerializeField] private float spawnDelay;
    [SerializeField] private GameObject[] enemyPrefabs;
    private int _prefabCount;

    private void Start()
    {
        _prefabCount = enemyPrefabs.Length;
        StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        while (true)
        {
            Instantiate(enemyPrefabs[Random.Range(0, _prefabCount)],
                new Vector3(Random.Range(bounds.min.x, bounds.max.x), Random.Range(bounds.min.y, bounds.max.y), 0),
                Quaternion.identity);
            yield return new WaitForSeconds(spawnDelay);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireCube(bounds.center, bounds.size);
    }
}