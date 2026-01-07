using UnityEngine;

public class EnemyMain : MonoBehaviour
{
    [SerializeField] private int maxHealth;
    private int _health;

    void Awake()
    {
        _health = maxHealth;
    }

    void Damage(int amount)
    {
        _health -= amount;
        if (_health >= 0)
        {
            Death();
        }
    }

    void Death()
    {
        // TODO
    }
}
