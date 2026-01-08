using UnityEngine;

public class SpriteStack : MonoBehaviour
{
    [SerializeField] private GameObject[] layers;
    [SerializeField] private float vMult;
    [SerializeField] private float hMult;

    private int _layerCount;
    private Vector2[] _individualMults;
    private Transform[] _transforms;

    void Awake()
    {
        _layerCount = layers.Length;
        _transforms = new Transform[_layerCount];
        _individualMults = new Vector2[_layerCount];

        for (int i = 0; i < _layerCount; i++)
        {
            _transforms[i] = layers[i].transform;
            _individualMults[i] = layers[i].GetComponent<SpriteStackingProperties>().multipliers;
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

    void ComputeParallax(Transform layerTransform, Vector2 currentPos, Vector2 mults)
    {
        layerTransform.localPosition =
            new Vector2(currentPos.x * hMult * 0.1f, currentPos.y * vMult * 0.1f) * mults;
    }
}