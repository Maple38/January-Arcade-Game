using UnityEngine;

public class SpriteStack : MonoBehaviour
{
    // Constant values for horizontal and vertical multipliers
    private const float VMult = 0.175f;
    private const float HMult = 0.1f;
    // Select the target gameobjects and their respective multipliers via the inspector
    [SerializeField] private GameObject[] layers;
    [SerializeField] private float[] multipliers;
    private float[] _individualMults;

    private int _layerCount;
    private Transform[] _transforms;

    private void Awake()
    {
        _layerCount = layers.Length;
        // Initialize arrays with a length determined by the amount of layers
        _transforms = new Transform[_layerCount];
        _individualMults = new float[_layerCount];

        // Cache references to the objects' transforms, to avoid having to look them up on each update
        for (var i = 0; i < _layerCount; i++) _transforms[i] = layers[i].transform;

        // The original multipliers array stores each item's individual multiplier, this array adds on all previous multipliers too 
        _individualMults[0] = multipliers[0];
        for (var i = 1; i < _layerCount; i++) _individualMults[i] = _individualMults[i - 1] + multipliers[i];
    }

    // Execute the main loop in LateUpdate() rather than Update() so that it happens after all the physics/movement
    private void LateUpdate()
    {
        Vector2 position = transform.position;
        for (var i = 0; i < _layerCount; i++) ComputeParallax(_transforms[i], position, _individualMults[i]);
    }

    // Parallax code
    private void ComputeParallax(Transform layerTransform, Vector2 currentPos, float mult)
    {
        layerTransform.localPosition =
            new Vector2(currentPos.x * HMult * 0.1f, currentPos.y * VMult * 0.1f) * mult;
    }
}