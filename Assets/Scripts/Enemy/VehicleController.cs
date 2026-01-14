using UnityEngine;
using UnityEngine.Serialization;

public class VehicleController : MonoBehaviour
{
    [SerializeField] private bool renderGizmos;
    [SerializeField] private float accelerationMax;
    [SerializeField] private float speedMax;
    [SerializeField] private float wheelAngleMax;
    [SerializeField] private float wheelOffsetFront;
    [SerializeField] private float wheelOffsetBack;
    [SerializeField] private float velocityMaxDelta;
    private Vector2 _velDesired;
    private Vector2 _velCurrent;
    private float _acceleration;
    
    private float _wheelAngle;
    private float _wheelBase;
    private float _rBack; // Turn radius according to the rear wheel axle
    private float _rBackMax; // Turn radius with wheels maximally angled

    void Update()
    {
        // Calculate the turning radius based on Ackerman's formula thingy
        _rBack = _wheelBase * Mathf.Tan(_wheelAngle * Mathf.Deg2Rad);
        
        CalculateMovement();
        if (Mathf.Abs(_wheelAngle) > 0.1f)
        {
            RotateWithMovement();
        }
        transform.Translate(_velCurrent * Time.deltaTime);
    }

    private void CalculateMovement()
    {
        _velDesired += _acceleration * ((Vector2)transform.up / Mathf.Cos(Mathf.Deg2Rad * _wheelAngle));
        _velDesired = Vector2.ClampMagnitude(_velDesired, speedMax);
        _velCurrent = Vector2.MoveTowards(_velCurrent, _velDesired, velocityMaxDelta);
    }

    public void Steer(float euler)
    {
        _wheelAngle = Mathf.Clamp(euler, -wheelAngleMax, wheelAngleMax);
    }

    public void Throttle(float value)
    {
        _acceleration = Mathf.Clamp01(value) * accelerationMax;
    }

    void RotateWithMovement()
    {
        var rotationAmount = Time.deltaTime * _velCurrent.magnitude / _rBack;
        transform.Rotate(transform.up, rotationAmount);
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
            Vector2 side;
            if (_wheelAngle < 0f)
            {
                side = transform.right * -1;
            }
            else
            {
                side = transform.right;
            }

            Vector2 axleFront = transform.position + transform.up * wheelOffsetFront;
            Vector2 axleBack = transform.position + transform.up * wheelOffsetBack;
            Vector2 turnPointMax = axleBack + side * _rBackMax;
            Vector2 turnPoint = axleBack + side * _rBack;

            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, _velCurrent);
            Gizmos.DrawLine(transform.position, _velDesired);
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(_velCurrent, _velDesired);

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