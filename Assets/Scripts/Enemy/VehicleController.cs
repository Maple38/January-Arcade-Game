using UnityEngine;
using UnityEngine.Serialization;

public class VehicleController : MonoBehaviour
{
    [SerializeField] private bool renderGizmos;
    [SerializeField] private float linearAccel;
    [SerializeField] private float speedMax;
    [SerializeField] private float wheelAngleMax;
    [SerializeField] private float wheelOffsetFront;
    [SerializeField] private float wheelOffsetBack;
    private Vector2 _desiredDir;
    private float _wheelAngle;
    private float _wheelBase;
    private float _rBack; // Turn radius according to the rear wheel axle
    private float _rBackMax;
    
    void Start()
    {
    }

    void Update()
    {
        // Calculate the turning radius based on Ackerman's formula thingy
        _rBack = _wheelBase * Mathf.Tan(_wheelAngle * Mathf.Deg2Rad);
        
        if (Mathf.Abs(_wheelAngle) > 0.1f)
        {
            RotateWithMovement();
        }
    }

    void RotateWithMovement()
    {
        // TODO
        // ∆rotation = ∆x/r
    }

    void OnValidate()
    {
        _wheelBase = Mathf.Abs(wheelOffsetFront) + Mathf.Abs(wheelOffsetBack);
        _rBackMax = _wheelBase / Mathf.Tan(wheelAngleMax * Mathf.Deg2Rad);
        _rBack = _wheelBase / Mathf.Tan(_wheelAngle * Mathf.Deg2Rad);
    }

    void OnDrawGizmosSelected()
    {
        if (renderGizmos)
        {
            var rFrontMax = _wheelBase / Mathf.Sin(wheelAngleMax * Mathf.Deg2Rad);
            var rFront = _wheelBase / Mathf.Sin(_wheelAngle * Mathf.Deg2Rad);

            Vector2 axleFront = transform.position + transform.up * wheelOffsetFront;
            Vector2 axleBack = transform.position + transform.up * wheelOffsetBack;

            Vector2 turnPointMax = axleBack + (Vector2)transform.right * _rBackMax;
            Vector2 turnPoint = axleBack + (Vector2)transform.right * _rBack;

            Gizmos.color = Color.magenta;
            Gizmos.DrawLine(axleFront, axleBack);

            Gizmos.color = Color.blue;
            Gizmos.DrawLine(axleFront, turnPointMax);
            Gizmos.DrawLine(axleBack, turnPointMax);
            Gizmos.DrawWireSphere(turnPointMax, rFrontMax);
            Gizmos.DrawWireSphere(turnPointMax, _rBackMax);

            Gizmos.color = Color.green;
            Gizmos.DrawLine(axleFront, turnPoint);
            Gizmos.DrawLine(axleBack, turnPoint);
            Gizmos.DrawWireSphere(turnPoint, rFront);
            Gizmos.DrawWireSphere(turnPoint, _rBack);
        }
    }
}