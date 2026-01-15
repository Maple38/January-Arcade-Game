using System;
using UnityEngine;
using UnityEngine.Serialization;

public class VehicleBrainDirect : MonoBehaviour
{
    private Vector2 _targetPos = new (-50, -20);
    private VehicleController _controller;
    private Vector2 _targetDir;
    
    void Start()
    {
        _controller = GetComponent<VehicleController>();
        _controller.Throttle(1);
    }
    
    void Update()
    {
        _controller.Steer(CalculateSteering());
        if (Input.GetMouseButtonDown(0))
        {
            _targetPos = (Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }
    }

    private float CalculateSteering()
    {
        _targetDir = (_targetPos - (Vector2)transform.position).normalized;
        return Vector2.SignedAngle(transform.up, _targetDir);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(transform.position, transform.position + (Vector3)_targetDir * 3);
        Gizmos.DrawSphere(_targetPos, 0.1f);
    }
}
