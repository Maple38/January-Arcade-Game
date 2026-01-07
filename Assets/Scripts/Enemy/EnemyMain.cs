using UnityEngine;

public class EnemyMain : MonoBehaviour
{
    [SerializeField] private int maxHealth;
    public int contactDamage;
    private int _health;

    private void Awake()
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