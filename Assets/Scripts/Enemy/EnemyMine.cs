using UnityEngine;

public class EnemyMine : EnemyMain
{
    private void Update()
    {
        transform.Translate(-transform.right * (Time.deltaTime * GameManager.Instance.globalScrollSpeed));
    }

    // Mines shouldn't spawn powerups
    protected override void TrySpawnPowerup()
    {
    }
}