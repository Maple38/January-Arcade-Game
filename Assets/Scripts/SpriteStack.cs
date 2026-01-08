using UnityEngine;

public class SpriteStack : MonoBehaviour
{
    [SerializeField] private Transform[] transforms;
    [SerializeField] private float vMult;
    [SerializeField] private float hMult;

    private float[] _initialZs;
    private int _layerCount;
    
    void Awake()
    {
        _layerCount = transforms.Length;
        _initialZs = new float[_layerCount];
        for (int i = 0; i < _layerCount; i++)
        {
            _initialZs[i] = transforms[i].position.z;
        }
    }

    void LateUpdate()
    {
        Vector2 position = transform.position;
        for (int i = 0; i < _layerCount; i++)
        {
            ComputeParallax(transforms[i], _initialZs[i], position);
        }
    }

    void ComputeParallax(Transform layerTransform, float zCoord, Vector2 currentPos)
    {
        layerTransform.localPosition =
            new Vector3(currentPos.x * hMult * zCoord * 0.1f, currentPos.y * vMult * zCoord * 0.1f, zCoord);
    }

}
