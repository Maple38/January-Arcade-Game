using UnityEngine;

public class EnemyMine : EnemyMain
{
    private void Update()
    {
        transform.Translate(-transform.right * (Time.deltaTime * GameManager.Instance.globalScrollSpeed));
    }
}