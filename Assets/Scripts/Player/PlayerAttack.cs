using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private Rigidbody2D _rb;
    private PlayerMain _playerMain;

    [Header("Targeting")]
    [SerializeField] private float targetingRange; // The range that the enemy must be in to be targeted
    [SerializeField] private LayerMask layerMask; // Layers to target with the ram attack
    [SerializeField] private float scanRange; // Range of the raycast that looks for targets
    [SerializeField] private GameObject crosshairObject; // GameObject for the crosshair TODO: Make this a prefab and spawn at script start
    private float _targetingGracePeriod; // If the target leaves the crosshair momentarily, attack should stay prepped
    private Vector2 _targetPos;
    private bool _targetReady;
    private SpriteRenderer _crosshairSr;

    [Header("Ramming")]
    [SerializeField] private float ramChargeMin; // Minimum charge time before the ram attack is ready
    [SerializeField] private float ramChargeMax; // Time for the ram attack to reach full charge
    [SerializeField] private int ramPowerMin; // Ram power at minimum charge
    [SerializeField] private int ramPowerMax; // Ram power at maximum charge
    [SerializeField] private float ramCooldownMax; // Ram attack cooldown time
    [DoNotSerialize] public float ramChargeMult = 1; // Public so powerups can change it
    [SerializeField] private float ramSpeedMult; // Multiplier for ramming movement speed
    private float _ramCharge; // Progress in charging up the ram attack at any given moment
    private float _ramCooldownCurrent;

    // We want PlayerMain.cs to be able to access these, but not change them
    public int RamPowerCurrent { get; private set; }
    public bool RamInProgress { get; private set; }

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _crosshairSr = crosshairObject.GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        TargetScan();
        if (_ramCooldownCurrent >= 0)
        {
            // Tick away at the ram attack's cooldown
            _ramCooldownCurrent -= Time.deltaTime * ramChargeMult;
        }

        if (_ramCharge >= 0)
        {
            // Decay the ram charge at all times. Charging is multiplied by 2 to compensate for this.
            _ramCharge -= Time.deltaTime;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawRay(transform.position, Vector2.up * scanRange);
        Gizmos.color = Color.cyan;
        Gizmos.DrawRay(transform.position, Vector2.up * targetingRange);

        // Gizmos.color = Color.red;
        // Gizmos.DrawRay(transform.position,
        //     new Vector2(Mathf.Cos((transform.rotation.eulerAngles.z + 90) * Mathf.Deg2Rad),
        //         Mathf.Sin((transform.rotation.eulerAngles.z + 90) * Mathf.Deg2Rad)) * 4);
    }

    // Scan for targets
    private void TargetScan()
    {
        var zRotation = (transform.localEulerAngles.z + 90) * Mathf.Deg2Rad;
        // Fire a ray and save its hit info to scanResult
        var scanResult = Physics2D.Raycast
        (new Vector2(transform.position.x, transform.position.y),
            new Vector2(Mathf.Cos(zRotation), Mathf.Sin(zRotation)),
            scanRange, layerMask);
        // If the scanResult includes a collider this will be true, indicating a successful hit
        if (scanResult.collider)
        {
            // Update the internal targeting info and the crosshair's position
            _targetPos = scanResult.collider.bounds.center;
            crosshairObject.transform.position = _targetPos;
            _crosshairSr.enabled = true;
            _targetReady = true; // We have a target currently in sight and in range
            _targetingGracePeriod = 0.2f; // Give a 200ms grace period in which the target could disappear without disrupting anything
        }
        else
        {
            _crosshairSr.enabled = false;
            _targetReady = false;
            _targetingGracePeriod -= Time.deltaTime;
        }
    }

    public void ChargeRam()
    {
        if (_ramCooldownCurrent <= 0)
        {
            // Charging speed is multiplied by 2 to account for the decay
            _ramCharge += Time.deltaTime * 2 * ramChargeMult;
        }

        if (_ramCharge >= ramChargeMax)
        {
            // TODO update animation
        }
        else if (_ramCharge >= ramChargeMin)
        {
            // TODO update animation
        }
    }

    public void TriggerRam()
    {
        // First check if either there's a target in sight, or if the grace period is active...
        if (_targetReady || _targetingGracePeriod >= 0)
        {
            // ...then if the charge meets the minimum...
            if (_ramCharge >= ramChargeMin)
            {
                // ...check if it meets the maximum too...
                if (_ramCharge >= ramChargeMax)
                { // ... if so then we set the ram power to a high value...
                    RamPowerCurrent = ramPowerMax;
                }
                else
                { // ...but if not, a low value.
                    RamPowerCurrent = ramPowerMin;
                }

                RamInProgress = true;
                _ramCooldownCurrent = ramCooldownMax;
                StartCoroutine(RamCoroutine(transform.position, _targetPos));
            }
        }
    }

    // Coroutine for handling the movement when ramming
    private IEnumerator RamCoroutine(Vector2 start, Vector2 end)
    {
        float moveProgress = 0;
        while (moveProgress <= 1f)
        {
            var moveDelta = Time.deltaTime * ramSpeedMult;
            moveProgress += moveDelta;
            transform.position = Vector2.LerpUnclamped(start, end, moveProgress);
            yield return new WaitForFixedUpdate();
        }

        RamInProgress = false;
    }
}