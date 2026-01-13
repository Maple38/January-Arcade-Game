using UnityEngine;

public class SpriteStack : MonoBehaviour
{
    [SerializeField] private GameObject[] layers;
    [SerializeField] private float[] multipliers;
    private const float VMult = 0.175f;
    private const float HMult = 0.1f;

    private int _layerCount;
    private float[] _individualMults;
    private Transform[] _transforms;

    void Awake()
    {
        _layerCount = layers.Length;
        _transforms = new Transform[_layerCount];
        _individualMults = new float[_layerCount];

        for (int i = 0; i < _layerCount; i++)
        {
            _transforms[i] = layers[i].transform;
        }

        _individualMults[0] = multipliers[0];
        for (int i = 1; i < _layerCount; i++)
        {
            _individualMults[i] = _individualMults[i - 1] + multipliers[i];
        }
    }

    void LateUpdate()
    {
        Vector2 position = transform.position;
        for (int i = 0; i < _layerCount; i++)
        {
            ComputeParallax(_transforms[i], position, _individualMults[i]);
        }
    }

    void ComputeParallax(Transform layerTransform, Vector2 currentPos, float mult)
    {
        layerTransform.localPosition =
            new Vector2(currentPos.x * HMult * 0.1f, currentPos.y * VMult * 0.1f) * mult;
    }
}