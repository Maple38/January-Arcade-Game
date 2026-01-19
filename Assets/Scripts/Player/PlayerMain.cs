using UnityEngine;

public class PlayerMain : MonoBehaviour
{
    [SerializeField] private float damageCooldown; // Time the player is invulnerable for after taking damage
    [SerializeField]
    private int
        maxHealth = 6; // This is the maximum health value. I dearly hope this doesn't need any further explaining.
    [SerializeField] private int contactDamage; // The amount of damage enemies should take on collision
    [SerializeField] private float kbPowerDefault; // The knockback to apply to enemies on collision
    [SerializeField] private HealthbarController healthbar; // Reference to the healthbar
    private int _health;
    private float _iFrames; // Not actually measured in frames, it's measured in time
    private float _kbPower; // The knockback power actually used by knockback code. Might be modified by powerups.
    private PlayerAttack _playerAttack;
    private PlayerMovement _playerMovement;

    private void Awake()
    {
        _playerAttack = GetComponent<PlayerAttack>();
        _playerMovement = GetComponent<PlayerMovement>();
        _health = maxHealth;
        _kbPower = kbPowerDefault;
    }

    private void Start()
    {
        // Running this in Start() so the healthbar script can load references to the hearts in Awake()
        healthbar.UpdateHearts(_health); // Make sure the correct number of hearts are displayed
    }

    private void Update()
    {
        if (_iFrames > 0)
        {
            _iFrames -= Time.deltaTime;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        var enemy = other.gameObject;
        if (enemy.CompareTag("Enemy"))
        {
            var enemyMain = enemy.GetComponent<EnemyMain>();
            // If the player is ramming, don't take damage
            if (_playerAttack.RamInProgress)
            {
                enemyMain.Damage(_playerAttack.RamPowerCurrent);
                Invincibility(0.1f); // Become invincible for 100ms to protect from any damage sources unaccounted for
                // TODO vfx + sound
            }
            // Otherwise, we both take damage
            else
            {
                Damage(enemyMain.contactDamage);
                enemyMain.Damage(contactDamage);

                // Apply knockback with given power at the angle of collision
                var collisionAngle = Vector2.Angle(transform.position, other.transform.position) * Mathf.Deg2Rad;
                enemyMain.Knockback(new Vector2(Mathf.Cos(collisionAngle), Mathf.Sin(collisionAngle) * _kbPower));
            }
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

    // Become invincible for a certain amount of time. This code makes sure it doesn't get shortened by mistake,
    // and that it's an overwrite so we don't get scripts accidentally adding lots and lots of time to it
    private void Invincibility(float duration)
    {
        _iFrames = Mathf.Max(_iFrames, duration);
    }

    // Public function to take damage, which can be called to damage the player. Yeah, it's a damage function.
    public void Damage(int amount)
    {
        Invincibility(damageCooldown);
        _health = Mathf.Clamp(_health - amount, 0, maxHealth);
        healthbar.UpdateHearts(_health);
        if (_health <= 0)
        {
            Death();
        }
    }

    private void Death()
    {
        GameManager.Instance.TriggerLoss();
        gameObject.SetActive(false);
    }
}