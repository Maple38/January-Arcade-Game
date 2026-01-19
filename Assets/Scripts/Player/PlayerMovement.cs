using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float impulseSpeed; // The strength to "impulse" the player by when a key is pressed
    [SerializeField] private Vector2 impulseAxisMults; // The X and Y multipliers for impulses
    [DoNotSerialize] public float speedMult = 1; // Public variable, allows for other scripts to adjust the speed
    private Rigidbody2D _rb;
    private Animator _anim;
    private int _doBoostHash;

    private void Start()
    {
        _anim = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
    }


    // Triggered by an external script that handles inputs
    public void Impulse(float impulseAngle)
    {
        // Calculate force to apply to player...
        var force = new Vector2(
            Mathf.Cos(Mathf.Deg2Rad * (impulseAngle - transform.eulerAngles.z))
            * impulseSpeed * speedMult * impulseAxisMults.x,
            Mathf.Sin(Mathf.Deg2Rad * (impulseAngle - transform.eulerAngles.z))
            * impulseSpeed * speedMult * impulseAxisMults.y);
        // ...and apply it
        _rb.AddForce(force,
            ForceMode2D.Impulse);

        _anim.SetTrigger("doBoost");
    }
}