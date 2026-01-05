using UnityEngine;
using UnityEngine.Serialization;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float impulseSpeed;

    [FormerlySerializedAs("turnStep")] [SerializeField]
    private float angleIncrement;

    [SerializeField] private float forwardImpulseMult;
    [SerializeField] private int maxAngleIndex;
    [SerializeField] private float driftBackSpeed;

    private Rigidbody2D _rb;
    private float _angleIndex;
    private float _angleMult;
    private Bounds _bounds;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _bounds = GetComponent<BoundaryScript>().bounds;
    }

    // Update is called once per frame
    void Update()
    {
        _angleIndex = Mathf.Clamp(_angleIndex, -maxAngleIndex, maxAngleIndex);
        transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, _angleIndex * angleIncrement);

        if (_rb.linearVelocityY < 0.07 && transform.position.y > _bounds.min.y + driftBackSpeed * 0.25)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y - (driftBackSpeed * Time.deltaTime),
                transform.position.z);
        }
    }

    public void AngleAdd(int indexIncrement)
    {
        _angleIndex = Mathf.Round(_angleIndex + indexIncrement);
    }

    public void AngleSet(int index)
    {
        _angleIndex = index;
    }

    public void Impulse(float impulseAngle)
    {
        float internalImpulseSpeed;

        if (impulseAngle == 0)
        {
            internalImpulseSpeed = impulseSpeed * forwardImpulseMult;
        }
        else
        {
            internalImpulseSpeed = impulseSpeed;
        }

        Vector2 force = new Vector2(
            -Mathf.Sin(Mathf.Deg2Rad * (impulseAngle + transform.eulerAngles.z)) * internalImpulseSpeed,
            Mathf.Cos(Mathf.Deg2Rad * (impulseAngle + transform.eulerAngles.z)) * internalImpulseSpeed);

        _rb.AddForce(force,
            ForceMode2D.Impulse);
    }
}