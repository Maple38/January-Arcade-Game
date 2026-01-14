using System;
using UnityEngine;
using UnityEngine.Serialization;

public class VehicleBrainDirect : MonoBehaviour
{
    private Vector2 _targetPos = new (-5, 0);
    private VehicleController _controller;
    [SerializeField] private float throttleOverTen;
    
    void Start()
    {
        _controller = GetComponent<VehicleController>();
    }
    
    void Update()
    {
        _controller.Steer(CalculateSteering());
        _controller.Throttle(throttleOverTen/10);
        if (Input.GetMouseButtonDown(0))
        {
            _targetPos = (Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }
    }

    private float CalculateSteering()
    {
        var targetDir = _targetPos - (Vector2)transform.position;
        return Vector2.SignedAngle(transform.up, targetDir);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere(_targetPos, 0.1f);
    }
}
