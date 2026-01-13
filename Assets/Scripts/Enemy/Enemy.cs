using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int maxHealth;
    [SerializeField] private int points;
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

    protected void Death()
    {
        GameManager.Instance.AddScore(points);
        DeathAnimation();
        Despawn();
    }

    protected virtual void DeathAnimation()
    {
        // TODO
    }
    
    // Separated from Death(), this function handles cleanup and cleanly getting rid of the object, without extras
    public void Despawn()
    {
        
    }

    public void Knockback(Vector2 vector)
    {
    }
}