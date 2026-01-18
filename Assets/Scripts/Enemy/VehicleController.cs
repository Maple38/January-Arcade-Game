using UnityEngine;

public class VehicleController : MonoBehaviour
{
    [Header("Movement Settings")]
    [Range(0f, 10f)] [SerializeField] private float traction;
    [SerializeField] private float accelerationMax;
    [SerializeField] private float speedMax;
    [SerializeField] private float wheelAngleMax;
    
    [Header("Other Configs")]
    [SerializeField] private bool renderGizmos;
    [SerializeField] private float wheelOffsetFront;
    [SerializeField] private float wheelOffsetBack;
    
    private Vector2 _velDesired;
    private Vector2 _velCurrent;
    private float _acceleration;
    private Vector2 _steeringVector;
    private float _wheelAngle;
    private float _wheelBase; // Distance between front and back axles
    private float _rBack; // Turn radius according to the rear wheel axle
    private float _rBackMax; // Turn radius with wheels maximally angled
    private float _shockTime; // Used to pause physics when "shocked" TODO

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
        // Multiply the up vector by a quaternion representing the wheel angle, to rotate it in that direction
        _steeringVector = Quaternion.Euler(0, 0, _wheelAngle) * transform.up;
        // Calculate the ideal velocity vector
        _velDesired = _velCurrent.magnitude * _steeringVector;
        // Interpolate the current velocity towards the ideal/desired velocity, with respect to traction
        _velCurrent = Vector2.Lerp(_velCurrent, _velDesired, traction * Time.deltaTime);
        // Accelerate in the direction of steering and clamp magnitude
        _velCurrent += _steeringVector * (_acceleration * Time.deltaTime);
        _velCurrent = Vector2.ClampMagnitude(_velCurrent, speedMax);
        // Move according to the calculated current velocity, multiplied by time
        transform.position += (Vector3)(_velCurrent * Time.deltaTime);

        // Don't want to divide by zero, and we can skip this calculation anyway if the wheels are straight
        if (!Mathf.Approximately(_wheelAngle, 0)) 
        {
            // Rotation of a moving vehicle = distance delta / turning radius
            // Note: Rotation is calculated in radians, needs to be converted to degrees
            transform.Rotate(transform.forward,
                Time.deltaTime * (Mathf.Rad2Deg * _velCurrent.magnitude / _rBack));
        }
    }

    public void ApplyForce(Vector2 vector)
    {
        _velCurrent += vector;
    }

    // To be called by the "brain" script in order to control the vehicle's steering
    public void Steer(float euler)
    {
        _wheelAngle = Mathf.Clamp(euler, -wheelAngleMax, wheelAngleMax);
    }

    // Used by the brain to control the throttle, determining acceleration
    public void Throttle(float value)
    {
        _acceleration = Mathf.Clamp01(value) * accelerationMax;
    }

    // OnValidate() is called after editing values in the inspector
    // This is all stuff that could have been done in Start() or Awake(), but doing it here instead so gizmos always work
    void OnValidate()
    {
        // Set the wheel base
        _wheelBase = Mathf.Abs(wheelOffsetFront) + Mathf.Abs(wheelOffsetBack);
        // _rBackMax is UNSIGNED where as _rBack is SIGNED
        _rBackMax = Mathf.Abs(_wheelBase / Mathf.Tan(wheelAngleMax * Mathf.Deg2Rad));
        _rBack = _wheelBase / Mathf.Tan(_wheelAngle * Mathf.Deg2Rad);
    }

    void OnDrawGizmosSelected()
    {
        if (renderGizmos)
        {
            // The next few blocks of code just compute various gizmo-related values
            // Some could be made into properties rooted in the class rather than the function, but for the sake of simplicity
            // I'm just keeping them this way. It would offer a negligible performance impact, and when performance
            // is an issue, then gizmos shouldn't be enabled at all so this code would be redundant anyway.
            var rFrontMax = _wheelBase / Mathf.Sin(wheelAngleMax * Mathf.Deg2Rad);
            var rFront = _wheelBase / Mathf.Sin(_wheelAngle * Mathf.Deg2Rad);
            var turnSign = Mathf.Sign(_wheelAngle);

            Vector2 axleFront = transform.position + transform.up * wheelOffsetFront;
            Vector2 axleBack = transform.position + transform.up * wheelOffsetBack;
            Vector2 turnPointMax = axleBack - (Vector2)transform.right * turnSign * _rBackMax;
            Vector2 turnPoint = axleBack - (Vector2)transform.right * _rBack;
            
            Vector2 velCurrentPos = (Vector2)transform.position + _velCurrent;
            Vector2 velDesiredPos = (Vector2)transform.position + _velDesired;

            Gizmos.color = Color.red; // Shows the desired velocity and current velocity vectors
            Gizmos.DrawLine(transform.position, velCurrentPos);
            Gizmos.DrawLine(transform.position, velDesiredPos);
            
            Gizmos.color = Color.yellow; // Shows the difference between current and desired velocity vectors
            Gizmos.DrawLine(velCurrentPos, velDesiredPos);

            Gizmos.color = Color.white; // Shows the direction the wheels are angled
            Gizmos.DrawLine(transform.position, (Vector2)transform.position + _steeringVector * 3);

            Gizmos.color = Color.magenta; // This is used when setting the wheel offsets
            Gizmos.DrawLine(axleFront, axleBack);
            
            // Simplified turning radius gizmos
            Gizmos.color = Color.green; // Draws a circle representing the turning radii, as well as a line to the back axle
            Gizmos.DrawWireSphere(turnPointMax, _rBackMax);
            Gizmos.DrawLine(axleBack,turnPointMax);
            if (Mathf.Abs(_wheelAngle) > 0f)
            {
                Gizmos.DrawWireSphere(turnPoint, Mathf.Abs(_rBack));
            }
        }
    }
}