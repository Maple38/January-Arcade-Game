using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerAttack : MonoBehaviour
{
    private Rigidbody2D _rb;

    [Header("Targeting Settings")] 
    [SerializeField] private float targetingRange;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private float scanRange;
    [SerializeField] private GameObject crosshairObject;
    [SerializeField] private float ramSpeedMult;
    
    private float _targetingGracePeriod;
    private Vector2 _targetPos;
    private SpriteRenderer _crosshairSr;
    private Boolean _targetReady;

    [Header("Ramming")] [SerializeField] private float ramChargeMin;
    [SerializeField] private float ramChargeMax;
    [SerializeField] private float ramPowerMin;
    [SerializeField] private float ramPowerMax;
    public float ramChargeMultDefault;
    
    public float RamPowerCurrent { get; private set; }
    public Boolean RamInProgress { get; private set; }
    [DoNotSerialize] public float ramChargeMult;
    private float _ramCooldownCurrent;
    private float _ramCooldownMax;
    private float _ramCharge;

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
            _ramCooldownCurrent -= Time.deltaTime;
        }

        if (_ramCharge >= 0)
        {
            // Tick down the ram charge at all times. Charging is multiplied by 2 to account for this.
            _ramCharge -= Time.deltaTime;
        }
    }

    private void TargetScan()
    {
        float zRotation = (transform.localEulerAngles.z + 90) * Mathf.Deg2Rad;
        RaycastHit2D scanResult = Physics2D.Raycast
        (new Vector2(transform.position.x, transform.position.y),
            new Vector2(Mathf.Cos(zRotation), Mathf.Sin(zRotation)),
            scanRange, layerMask);
        if (scanResult.collider)
        {
            _targetPos = scanResult.collider.bounds.center;
            crosshairObject.transform.position = _targetPos;
            _crosshairSr.enabled = true;
            _targetReady = true;
            _targetingGracePeriod = 0.2f;
        }
        else
        {
            _crosshairSr.enabled = false;
            _targetReady = false;
            _targetingGracePeriod -= Time.deltaTime;
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

    public void RamRelease()
    {
        if (_targetReady || _targetingGracePeriod >= 0)
        {
            if (_ramCharge >= ramChargeMin)
            {
                if (_ramCharge >= ramChargeMax)
                {
                    RamPowerCurrent = ramPowerMax;
                }
                else
                {
                    RamPowerCurrent = ramPowerMin;
                }

                RamInProgress = true;
                _ramCooldownCurrent = _ramCooldownMax;
                StartCoroutine(RamCoroutine(transform.position, _targetPos));
            }
        }
    }

    private IEnumerator RamCoroutine(Vector2 start, Vector2 end)
    {
        float moveProgress = 0;
        while (moveProgress <= 1f)
        {
            float moveDelta = Time.deltaTime * ramSpeedMult;
            moveProgress += moveDelta;
            transform.position = Vector2.LerpUnclamped(start, end, moveProgress);
            yield return new WaitForFixedUpdate();
        }

        RamInProgress = false;
    }
}