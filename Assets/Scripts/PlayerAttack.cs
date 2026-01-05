using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private float targetingRange;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private float scanRange;
    [SerializeField] private GameObject crosshairObject;
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
        RaycastHit2D scanResult = Physics2D.Raycast
            (new Vector2(this.transform.position.x, this.transform.position.y), Vector2.right, scanRange, layerMask);
        if (scanResult.collider)
        {
            _targetPos = scanResult.centroid;
            crosshairObject.transform.position = _targetPos;
            _crosshairSr.enabled = true;
        }
        else
        {
            _crosshairSr.enabled = false;
        }
    }
}