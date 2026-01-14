using UnityEngine;
using UnityEngine.Serialization;

public class Vehicle : MonoBehaviour
{
    [SerializeField] private bool renderGizmos;
    [SerializeField] private float linearAccel;
    [SerializeField] private float speedMax;
    [SerializeField] private float wheelAngleMax;
    [SerializeField] private float wheelOffsetFront;
    [SerializeField] private float wheelOffsetBack;
    private Vector2 _targetPos;
    private Vector2 _desiredDir;
    [SerializeField] private float _wheelAngle;
    private float _wheelBase;
    private float _turnRadius; // Turn radius according to the rear wheel axle
    private float _turnRadiusMax;

    void Start()
    {
        _turnRadiusMax = _wheelBase * Mathf.Tan(wheelAngleMax * Mathf.Deg2Rad);
        _targetPos = new Vector2(0, 0);
        _wheelBase = Mathf.Abs(wheelOffsetFront) + Mathf.Abs(wheelOffsetBack);
    }

    void Update()
    {
        // Calculate the turning radius based on Ackerman's formula thingy
        _turnRadius = _wheelBase * Mathf.Tan(_wheelAngle * Mathf.Deg2Rad);
        
        if (Mathf.Abs(_wheelAngle) > 0.1f)
        {
            RotateWithMovement();
        }
        
        
        if (Input.GetMouseButtonDown(0))
        {
            _targetPos = (Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }
    }

    void RotateWithMovement()
    {
        // TODO
        // ∆rotation = ∆x/r
    }

    void OnDrawGizmosSelected()
    {
        if (renderGizmos)
        {
            _wheelBase = Mathf.Abs(wheelOffsetFront) + Mathf.Abs(wheelOffsetBack);
            var rFrontMax = _wheelBase / Mathf.Sin(wheelAngleMax * Mathf.Deg2Rad);
            var rFront = _wheelBase / Mathf.Sin(_wheelAngle * Mathf.Deg2Rad);
            var rBackMax = _wheelBase / Mathf.Tan(wheelAngleMax * Mathf.Deg2Rad);
            var rBack = _wheelBase / Mathf.Tan(_wheelAngle * Mathf.Deg2Rad);

            Vector2 axleFront = transform.position + transform.up * wheelOffsetFront;
            Vector2 axleBack = transform.position + transform.up * wheelOffsetBack;

            Vector2 turnPointMax = axleBack + (Vector2)transform.right * rBackMax;
            Vector2 turnPoint = axleBack + (Vector2)transform.right * rBack;

            Gizmos.color = Color.magenta;
            Gizmos.DrawSphere(_targetPos, 0.1f);
            Gizmos.DrawLine(axleFront, axleBack);

            Gizmos.color = Color.blue;
            Gizmos.DrawLine(axleFront, turnPointMax);
            Gizmos.DrawLine(axleBack, turnPointMax);
            Gizmos.DrawWireSphere(turnPointMax, rFrontMax);
            Gizmos.DrawWireSphere(turnPointMax, rBackMax);

            Gizmos.color = Color.green;
            Gizmos.DrawLine(axleFront, turnPoint);
            Gizmos.DrawLine(axleBack, turnPoint);
            Gizmos.DrawWireSphere(turnPoint, rFront);
            Gizmos.DrawWireSphere(turnPoint, rBack);
        }
    }
}