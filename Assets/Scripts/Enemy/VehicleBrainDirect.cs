using UnityEngine;

public class VehicleBrainDirect : MonoBehaviour
{
    private Vector2 _targetPos;
    private VehicleController _controller;
    private Vector2 _targetDir;
    // private bool _controlTargetWithMouse = true;  // Controls mouse control, which I'm using for debugging

    void Awake()
    {
        _controller = GetComponent<VehicleController>(); // The script used to control the vehicle and calculate physics
        _targetPos = GameManager.Instance.FetchPlayerPos();
    }

    void Update()
    {
        _controller.Throttle(1); // For now just setting the throttle to maximum always
        _controller.Steer(CalculateSteering()); // Steer in the determined direction
        
        
        // Mouse based steering
        // if (Input.GetMouseButtonDown(0)) // Click to toggle controlling the target position with the mouse
        // {
        //     _controlTargetWithMouse = !_controlTargetWithMouse;
        // }
        // if (_controlTargetWithMouse) // If true then the target follows the mouse
        // {
        //     _targetPos = (Camera.main.ScreenToWorldPoint(Input.mousePosition));
        // }
    }

    void FixedUpdate()
    {
        _targetPos = GameManager.Instance.FetchPlayerPos();
    }

    // Returns a normalized Vector2 representing the direction of the target relative to the object
    private float CalculateSteering()
    {
        _targetDir = (_targetPos - (Vector2)transform.position).normalized;
        return Vector2.SignedAngle(transform.up, _targetDir); 
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan; // Draws a line representing the target direction, and a dot on the target
        Gizmos.DrawLine(transform.position, transform.position + (Vector3)_targetDir * 3);
        Gizmos.DrawSphere(_targetPos, 0.1f);
    }
}