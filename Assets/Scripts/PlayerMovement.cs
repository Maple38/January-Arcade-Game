using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float impulseSpeed;
    [FormerlySerializedAs("turnStep")] [SerializeField] private float angleIncrement;
    [SerializeField] private float forwardImpulseMult;
    [SerializeField] private int maxAngleIndex;
    [SerializeField] private float angleCorrectionAmount;
    [SerializeField] private GameObject targetObject;
    [SerializeField] private float moveSpeed;
    private Transform _targetPos;
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
        _targetPos = targetObject.GetComponent<Transform>();
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
    // void Update()
    // {
    //     // A and D
    //     if (_hardLeftAction.WasPressedThisFrame())
    //     {
    //         if (_modAction.IsPressed())
    //         {
    //             Impulse(90);
    //         }
    //         else
    //         { 
    //             _angleIndex = Mathf.Round(_angleIndex + 1);
    //         }
    //     }
    //     else if (_hardRightAction.WasPressedThisFrame())
    //     {
    //         if (_modAction.IsPressed())
    //         {
    //             Impulse(-90);
    //         }
    //         else 
    //         {
    //             _angleIndex = Mathf.Round(_angleIndex - 1);
    //         }
    //     }
    //
    //     // W and S
    //     if (_forwardAction.WasPressedThisFrame())
    //     {
    //         Impulse(0);
    //     }
    //     else if (_backAction.WasPressedThisFrame())
    //     {
    //         _angleIndex = 0;
    //     }
    //     
    //     _angleIndex = Mathf.Clamp(_angleIndex, -maxAngleIndex, maxAngleIndex);
    //     transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, _angleIndex * angleIncrement);
    // }

    void Update()
    {
        if (_hardLeftAction.IsPressed())
        {
            _targetPos.localPosition += Vector2.left * Time.deltaTime * moveSpeed;
        }
    }
    
    void Impulse(float impulseAngle)
    {
        float internalImpulseSpeed;
        float correctionMult = 0;
        
        if (impulseAngle == 0)
        {
            internalImpulseSpeed = impulseSpeed * forwardImpulseMult;
        }
        else
        {
            internalImpulseSpeed = impulseSpeed;
        }
        
        // Intuitively you'd assume that if the impulse and the rotation are in the same direction, they'd have the
        // same sign, but that's not the case as one operates clockwise and the other counterclockwise.
        // ReSharper disable once CompareOfFloatsByEqualityOperator
        if (Mathf.Sign(impulseAngle) == Mathf.Sign(_angleIndex))
        {
            // We're dashing in the same direction we're rotated, so don't correct as much.
            correctionMult = 1;
        }
        else
        {
            // If we're dashing in the opposite direction, correct twice as much, this feels better.
            correctionMult = 2;
        }
        
        if (_angleIndex > 0 + angleCorrectionAmount)
        {
            _angleIndex -= angleCorrectionAmount * correctionMult;
        }
        else if (_angleIndex < 0 - angleCorrectionAmount)
        {
            _angleIndex += angleCorrectionAmount * correctionMult;
        }
        else
        {
            _angleIndex = 0;
        }

        Vector2 force = new Vector2(
            -Mathf.Sin(Mathf.Deg2Rad * (impulseAngle + transform.eulerAngles.z)) * internalImpulseSpeed,
            Mathf.Cos(Mathf.Deg2Rad * (impulseAngle + transform.eulerAngles.z)) * internalImpulseSpeed);
        print(force);
        _rb.AddForce(force, 
            ForceMode2D.Impulse);
    }
}
