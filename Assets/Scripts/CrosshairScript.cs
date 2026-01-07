using UnityEngine;

public class CrosshairScript : MonoBehaviour
{
    [SerializeField] private float rotSpeed;

    private void FixedUpdate()
    {
        transform.Rotate(0, 0, rotSpeed);
    }
}