using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class HealthbarController : MonoBehaviour
{
    private HeartController[] _hearts;
    [SerializeField] private int testHealthValue; // For debugging purposes

    // For debugging purposes
    [ContextMenu("Apply Test Value")] 
    void TestHealth()
    {
        UpdateHearts(testHealthValue);
    }
    
    void Start()
    {
        _hearts = GetComponentsInChildren<HeartController>();
    }

    public void UpdateHearts(int health)
    {
        // Loops through each heart and allocates health to it
        foreach (HeartController heart in _hearts)
        {
            switch (health)
            {
                case >= 2:
                    heart.SetState(2);
                    health -= 2;
                    break;
                case 1:
                    heart.SetState(1);
                    health -= 1;
                    break;
                case >= 0:
                    heart.SetState(0);
                    break;
            }
        }
    }
}
