using UnityEngine;

public class DespawnScript : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            // If the enemy has an EnemyMain.cs script, then call the despawn function
            collision.GetComponent<EnemyMain>()?.Despawn();
        }
    }
}