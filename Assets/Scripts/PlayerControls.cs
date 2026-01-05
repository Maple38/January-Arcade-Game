using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControls : MonoBehaviour
{
    private PlayerMovement _playerMovement;
    
    private InputAction _hardLeftAction;
    private InputAction _softLeftAction;
    private InputAction _hardRightAction;
    private InputAction _softRightAction;
    private InputAction _forwardAction;
    private InputAction _backAction;
    private InputAction _modAction;

    void Start()
    {
        _playerMovement = GetComponent<PlayerMovement>();
        
        _hardLeftAction = InputSystem.actions.FindAction("HardLeft");
        _softLeftAction = InputSystem.actions.FindAction("SoftLeft");
        _hardRightAction = InputSystem.actions.FindAction("HardRight");
        _softRightAction = InputSystem.actions.FindAction("SoftRight");
        _forwardAction = InputSystem.actions.FindAction("Forward");
        _backAction = InputSystem.actions.FindAction("Back");
        _modAction = InputSystem.actions.FindAction("ModAction");
    }

    void Update()
    {
        // A and D
        if (_hardLeftAction.WasPressedThisFrame())
        {
            _playerMovement.Impulse(90);
        }
        else if (_hardRightAction.WasPressedThisFrame())
        {
            _playerMovement.Impulse(-90);
        }

        // Q and E
        if (_softLeftAction.IsPressed())
        {
            _playerMovement.AngleAdd(1);
        }
        else if (_softRightAction.IsPressed())
        {
            _playerMovement.AngleAdd(-1);
        }
        else
        {
            _playerMovement.AngleSet(0);
        }

        // W and S
        if (_forwardAction.WasPressedThisFrame())
        {
            _playerMovement.Impulse(0);
        }
        else if (_backAction.WasPressedThisFrame())
        {
            _playerMovement.Impulse(180);
        }
    }
}
