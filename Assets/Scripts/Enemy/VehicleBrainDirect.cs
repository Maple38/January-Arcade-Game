using System;
using UnityEngine;
using UnityEngine.Serialization;

public class VehicleBrainDirect : MonoBehaviour
{
    private Vector2 _targetPos = new (-5, 0);
    private VehicleController _controller;
    private Vector2 _targetDir;
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
