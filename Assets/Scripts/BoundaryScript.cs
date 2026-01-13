using UnityEngine;

public class BoundaryScript : MonoBehaviour
{
    public Bounds bounds;
    public bool boundaryEnabled = true;

    private void LateUpdate()
    {
        if (boundaryEnabled)
        {
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