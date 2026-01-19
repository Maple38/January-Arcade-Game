using UnityEngine;

public class BoundaryScript : MonoBehaviour
{
    // Variables are public to allow other scripts to control the boundary behavior
    public Bounds bounds;
    public bool boundaryEnabled = true;

    // Using LateUpdate() so this is done last after all the movement calculations
    private void LateUpdate()
    {
        if (boundaryEnabled)
        {
            // Clamp the object's position to the defined bounds
            transform.position = new Vector3(
                Mathf.Clamp(transform.position.x, bounds.min.x, bounds.max.x),
                Mathf.Clamp(transform.position.y, bounds.min.y, bounds.max.y),
                transform
                    .position.z);
        }
    }

    // Renders the boundary box when the object is selected
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(bounds.center, bounds.size);
    }
}