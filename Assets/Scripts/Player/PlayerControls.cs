using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControls : MonoBehaviour
{
    private PlayerAttack _playerAttack;
    private PlayerMovement _playerMovement;
    
    private InputAction _attackKey;
    private InputAction _upKey;
    private InputAction _downKey;
    private InputAction _leftKey;
    private InputAction _rightKey;

    private void Start()
    {
        _playerMovement = GetComponent<PlayerMovement>();
        _playerAttack = GetComponent<PlayerAttack>();

        // Cache references to each keybind
        _upKey = InputSystem.actions.FindAction("Up");
        _downKey = InputSystem.actions.FindAction("Down");
        _leftKey = InputSystem.actions.FindAction("Left");
        _rightKey = InputSystem.actions.FindAction("Right");
        _attackKey = InputSystem.actions.FindAction("Attack");
    }

    private void Update()
    {
        if (_upKey.WasPressedThisFrame())
        {
            _playerMovement.Impulse(0);
        }
        else if (_downKey.WasPressedThisFrame())
        {
            _playerMovement.Impulse(180);
        }

        
        if (_leftKey.WasPressedThisFrame())
        {
            _playerMovement.Impulse(90);
        }
        else if (_rightKey.WasPressedThisFrame())
        {
            _playerMovement.Impulse(270);
        }


        
        if (_attackKey.IsPressed())
        {
            _playerAttack.ChargeRam();
        }
        else if (_attackKey.WasReleasedThisFrame())
        {
            _playerAttack.TriggerRam();
        }
    }
}