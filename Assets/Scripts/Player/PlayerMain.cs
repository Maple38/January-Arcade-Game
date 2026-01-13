using System;
using UnityEngine;

public class PlayerMain : MonoBehaviour
{
    [SerializeField] private float damageCooldown;
    [SerializeField] private int maxHealth;
    [SerializeField] private int contactDamage;
    [SerializeField] private float kbPowerDefault;
    private int _health;
    private float _iFrames;
    private float _kbPower;
    private PlayerAttack _playerAttack;
    private PlayerMovement _playerMovement;

    private void Awake()
    {
        _playerAttack = GetComponent<PlayerAttack>();
        _playerMovement = GetComponent<PlayerMovement>();
        _health = maxHealth;
        _kbPower = kbPowerDefault;
    }

    private void Update()
    {
        if (_iFrames > 0)
        {
            _iFrames -= Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Slowdown"))
        {
            _playerMovement.speedMult = 0.5f;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Slowdown"))
        {
            _playerMovement.speedMult = 1f;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        var enemy = other.gameObject;
        if (enemy.CompareTag("Enemy"))
        {
            var enemyMain = enemy.GetComponent<Enemy>();
            if (_playerAttack.RamInProgress)
            {
                enemyMain.Damage(_playerAttack.RamPowerCurrent);
                Invincibility(0.1f);
                // TODO vfx + sound
            }
            else
            {
                Damage(enemyMain.contactDamage);
                enemyMain.Damage(contactDamage);

                var collisionAngle = Vector2.Angle(transform.position, other.transform.position) * Mathf.Deg2Rad;
                enemyMain.Knockback(new Vector2(Mathf.Cos(collisionAngle), Mathf.Sin(collisionAngle) * _kbPower));
            }
        }
    }

    private void Invincibility(float duration)
    {
        _iFrames = Mathf.Max(_iFrames, duration);
    }

    public void Damage(int amount)
    {
        Invincibility(damageCooldown);
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