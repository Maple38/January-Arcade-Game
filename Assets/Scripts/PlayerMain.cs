using UnityEngine;

public class PlayerMain: MonoBehaviour
{
    private int _health;
    [SerializeField] private int maxHealth;

    void Awake()
    {
        _health = maxHealth;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Enemy"))
        {
            EnemyMain enemyMain = col.gameObject.GetComponent<EnemyMain>();
        }
    }
    
    public void Damage(int amount)
    {
        _health += Mathf.Clamp(_health + amount, 0, maxHealth);
        if (_health <= 0)
        {
            Death();
        }
    }

    public void Death()
    {
        // TODO
    }
    
}