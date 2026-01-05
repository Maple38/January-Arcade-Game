using System;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private float targetingRange;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private float scanRange;
    [SerializeField] private GameObject crosshairObject;
    [SerializeField] private float ramRange;
    private Vector2 _targetPos;
    private SpriteRenderer _crosshairSr;

    void Awake()
    {
        _crosshairSr = crosshairObject.GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        TargetScan();
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
        }
        else
        {
            _crosshairSr.enabled = false;
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
}