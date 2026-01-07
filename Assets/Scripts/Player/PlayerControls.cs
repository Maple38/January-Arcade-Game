using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControls : MonoBehaviour
{
    private InputAction _backAction;
    private InputAction _forwardAction;

    private InputAction _hardLeftAction;
    private InputAction _hardRightAction;
    private InputAction _modAction;
    private PlayerAttack _playerAttack;
    private PlayerMovement _playerMovement;
    private InputAction _softLeftAction;
    private InputAction _softRightAction;

    private void Start()
    {
        _playerMovement = GetComponent<PlayerMovement>();
        _playerAttack = GetComponent<PlayerAttack>();

        _hardLeftAction = InputSystem.actions.FindAction("HardLeft");
        _softLeftAction = InputSystem.actions.FindAction("SoftLeft");
        _hardRightAction = InputSystem.actions.FindAction("HardRight");
        _softRightAction = InputSystem.actions.FindAction("SoftRight");
        _forwardAction = InputSystem.actions.FindAction("Forward");
        _backAction = InputSystem.actions.FindAction("Back");
        _modAction = InputSystem.actions.FindAction("ModAction");
    }

    private void Update()
    {
        // Q and E by default
        if (_hardLeftAction.WasPressedThisFrame())
        {
            _playerMovement.Impulse(90);
        }
        else if (_hardRightAction.WasPressedThisFrame())
        {
            _playerMovement.Impulse(-90);
        }

        // A and D by default
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

        // W
        if (_forwardAction.WasPressedThisFrame())
        {
            _playerMovement.Impulse(0);
        }

        if (_backAction.IsPressed())
        {
            _playerAttack.ChargeRam();
        }
        else if (_backAction.WasReleasedThisFrame())
        {
            _playerAttack.TriggerRam();
        }
    }
}