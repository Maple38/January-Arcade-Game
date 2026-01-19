using UnityEngine;

public class EnemyMine : EnemyMain
{
    void Update()
    {
        transform.Translate(-transform.right * (Time.deltaTime * GameManager.Instance.GlobalScrollSpeed));
    }
}
