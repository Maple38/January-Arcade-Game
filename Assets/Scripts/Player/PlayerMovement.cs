using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float impulseSpeed;
    [SerializeField] private Vector2 impulseAxisMults;
    [DoNotSerialize] public float speedMult = 1;
    private float _angleIndex;
    private float _angleMult;
    private Bounds _bounds;
    private Rigidbody2D _rb;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _bounds = GetComponent<BoundaryScript>().bounds;
    }

    // private void Update()
    // {
    //     // DriftBack();
    // }

    // private void DriftBack()
    // {
    //     if (Mathf.Abs(_rb.linearVelocityY) < 0.01 && transform.position.y > _bounds.min.y + driftBackSpeed * 0.25)
    //     {
    //         transform.position = new Vector3(transform.position.x,
    //             transform.position.y - driftBackSpeed * Time.deltaTime,
    //             transform.position.z);
    //     }
    // }

    public void Impulse(float impulseAngle)
    {
        var force = new Vector2(
            Mathf.Cos(Mathf.Deg2Rad * (impulseAngle - transform.eulerAngles.z))
            * impulseSpeed * speedMult * impulseAxisMults.x,
            Mathf.Sin(Mathf.Deg2Rad * (impulseAngle - transform.eulerAngles.z))
            * impulseSpeed * speedMult * impulseAxisMults.y);

        _rb.AddForce(force,
            ForceMode2D.Impulse);
    }
}