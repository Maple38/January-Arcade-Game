using System;
using UnityEngine;

public class BoundaryScript : MonoBehaviour
{
    public Bounds bounds;
    public Boolean boundaryEnabled = true;

    // Renders the boundary box when the object is selected
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(bounds.center, bounds.size);
    }

    private void Update()
    {
        if (boundaryEnabled)
        {
            transform.position = new Vector3(
                Mathf.Clamp(transform.position.x, bounds.min.x + bounds.center.x, bounds.max.x + bounds.center.x),
                Mathf.Clamp(transform.position.y, bounds.min.y + bounds.center.y, bounds.max.y + bounds.center.y),
                transform
                    .position.z);
        }
    }
}