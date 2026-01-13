using System;
using UnityEngine;

public class TempVehicleMovement : MonoBehaviour
{
    public float maxRotationIncrement;
    public Vector2 target;
    public float speed;
    private float _rotAngle;

    // Update is called once per frame
    void Update()
    {
        _rotAngle = Mathf.Clamp(Vector2.SignedAngle(transform.position, target), -maxRotationIncrement,
            maxRotationIncrement) * Time.deltaTime;
        print(_rotAngle);
        transform.Rotate(0, 0, _rotAngle);
        transform.Translate(Vector2.up * (speed * Time.deltaTime));
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawSphere(target, 0.25f);
    }
}