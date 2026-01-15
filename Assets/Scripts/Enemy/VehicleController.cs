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
    private Vector2 _steeringVector;
    
    private float _wheelAngle;
    private float _wheelBase;
    private float _rBack; // Turn radius according to the rear wheel axle
    private float _rBackMax; // Turn radius with wheels maximally angled
    private float _turnSide;

    void Update()
    {
        // Calculate the turning radius based on Ackerman's formula thingy
        _rBack = _wheelBase / Mathf.Tan(_wheelAngle * Mathf.Deg2Rad);
        
        // Skip complex physics calculations if they aren't needed
        if (_velCurrent.magnitude >= 0.1f || _acceleration > 0f)
        {
            CalculatePhysics();
        }
    }
    
    private void CalculatePhysics()
    {
        // Create a quaternion to represent the angle of the wheels
        var angleQuaternion = Quaternion.Euler(0, 0, _wheelAngle);
        // Multiply the "up" vector by our wheel angle quaternion to make it point in the proper direction
        _steeringVector = angleQuaternion * transform.up;
        // Apply acceleration in the desired direction
        _velDesired += Time.deltaTime * _acceleration * _steeringVector;
        // Clamp the vector's magnitude to the maximum speed
        _velDesired = Vector2.ClampMagnitude(_velDesired, speedMax);
        // The limit in change of velocity, this determines whether the car drifts or not
        _velCurrent = Vector2.MoveTowards(_velCurrent, _velDesired, Time.deltaTime * velocityMaxDelta);
        // Move according to the calculated current velocity, multiplied by time
        transform.position += (Vector3)(_velCurrent * Time.deltaTime);

        float rotationAmount = 0f;
        if (!Mathf.Approximately(_wheelAngle, 0)) // Don't want to divide by zero
        {
            // Rotation of a moving vehicle = distance delta / turning radius
            // Note: It seems this needs to be converted from radians to degrees 
            rotationAmount = Mathf.Rad2Deg * _velCurrent.magnitude / _rBack;
        }
        transform.Rotate(transform.forward, Time.deltaTime * _turnSide * rotationAmount);
    }

    public void Steer(float euler)
    {
        _wheelAngle = Mathf.Clamp(euler, -wheelAngleMax, wheelAngleMax);
        _turnSide = Mathf.Sign(_wheelAngle);
    }

    public void Throttle(float value)
    {
        _acceleration = Mathf.Clamp01(value) * accelerationMax;
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
            Vector2 velCurrentPos = (Vector2)transform.position + _velCurrent; 
            Vector2 velDesiredPos = (Vector2)transform.position + _velDesired;
            
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, velCurrentPos);
            Gizmos.DrawLine(transform.position, velDesiredPos);
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(velCurrentPos, velDesiredPos);

            Gizmos.color = Color.white;
            Gizmos.DrawLine(transform.position, (Vector2)transform.position + _steeringVector * 3);

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