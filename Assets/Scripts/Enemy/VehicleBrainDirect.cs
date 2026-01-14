using System;
using UnityEngine;

public class VehicleBrainDirect : MonoBehaviour
{
    private Vector2 _targetPos;
    
    void Start()
    {
        _targetPos = new Vector2(0, 0);
    }
    
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _targetPos = (Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere(_targetPos, 0.1f);
    }
}
