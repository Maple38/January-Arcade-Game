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
            var prefab = enemyPrefabs[Random.Range(0, _prefabCount)];
            var pos = new Vector3(Random.Range(bounds.min.x, bounds.max.x), Random.Range(bounds.min.y, bounds.max.y),
                0);
            // Instantiate the prefab, preserving the preset rotation
            Instantiate(prefab, pos, prefab.transform.rotation);
            yield return new WaitForSeconds(spawnDelay);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireCube(bounds.center, bounds.size);
    }
}