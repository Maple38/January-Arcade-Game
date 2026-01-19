using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class EnemyMain : MonoBehaviour
{
    [SerializeField] private int maxHealth;
    [SerializeField] private int points;
    public int contactDamage;
    private int _health;
    private VehicleController _vehicle;
    [SerializeField] private int powerupChance;

    private void Awake()
    {
        _health = maxHealth;
        // If the enemy is a vehicle, grab a reference to the controller
        TryGetComponent(out _vehicle);
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
        GameManager.Instance.EnemyDeathSound();
        Despawn();
    }

    protected virtual void DeathAnimation()
    {
        // TODO
    }

    // Separated from Death(), this function handles cleanup and cleanly getting rid of the object, without extras
    public void Despawn()
    {
        Destroy(gameObject);
    }

    protected virtual void TrySpawnPowerup()
    {
        if (Random.Range(0, powerupChance) == powerupChance)
        {
            GameManager.Instance.SpawnPowerup(transform.position);
        }
    }

    public void Knockback(Vector2 vector)
    {
        _vehicle?.ApplyForce(vector);
    }
}