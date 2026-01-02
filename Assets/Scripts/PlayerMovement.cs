using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float impulseSpeed;

    [FormerlySerializedAs("turnStep")] [SerializeField]
    private float angleIncrement;

    [SerializeField] private float forwardImpulseMult;
    [SerializeField] private int maxAngleIndex;
    [SerializeField] private float moveSpeed;
    private Rigidbody2D _rb;
    private float _angleIndex;
    private float _angleMult;

    private InputAction _hardLeftAction;
    private InputAction _softLeftAction;
    private InputAction _hardRightAction;
    private InputAction _softRightAction;
    private InputAction _forwardAction;
    private InputAction _backAction;
    private InputAction _modAction;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _hardLeftAction = InputSystem.actions.FindAction("HardLeft");
        _softLeftAction = InputSystem.actions.FindAction("SoftLeft");
        _hardRightAction = InputSystem.actions.FindAction("HardRight");
        _softRightAction = InputSystem.actions.FindAction("SoftRight");
        _forwardAction = InputSystem.actions.FindAction("Forward");
        _backAction = InputSystem.actions.FindAction("Back");
        _modAction = InputSystem.actions.FindAction("ModAction");
    }

    // Update is called once per frame
    void Update()
    {
        // A and D
        if (_hardLeftAction.WasPressedThisFrame())
        {
            Impulse(90);
        }
        else if (_hardRightAction.WasPressedThisFrame())
        {
            Impulse(-90);
        }

        // Q and E
        if (_softLeftAction.IsPressed())
        {
            _angleIndex = Mathf.Round(_angleIndex + 1);
        }
        else if (_softRightAction.IsPressed())
        {
            _angleIndex = Mathf.Round(_angleIndex - 1);
        }
        else
        {
            _angleIndex = 0;
        }

        // W and S
        if (_forwardAction.WasPressedThisFrame())
        {
            Impulse(0);
        }
        else if (_backAction.WasPressedThisFrame())
        {
            Impulse(180);
        }

        _angleIndex = Mathf.Clamp(_angleIndex, -maxAngleIndex, maxAngleIndex);
        transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, _angleIndex * angleIncrement);
    }

    void Impulse(float impulseAngle)
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