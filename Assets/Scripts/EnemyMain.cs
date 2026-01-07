using UnityEngine;

public class EnemyMain : MonoBehaviour
{
    [SerializeField] private int maxHealth;
    private int _health;

    void Awake()
    {
        _health = maxHealth;
    }
}
