using UnityEngine;

public class EnemyMain : MonoBehaviour
{
    [SerializeField] private int maxHealth;
    private int _health;
    public int contactDamage;

    void Awake()
    {
        _health = maxHealth;
    }

    public void Damage(int amount)
    {
        _health -= amount;
        if (_health >= 0)
        {
            Death();
        }
    }

    public void Death()
    {
        // TODO
    }

    public void Knockback(Vector2 vector)
    {
        
    }
}
