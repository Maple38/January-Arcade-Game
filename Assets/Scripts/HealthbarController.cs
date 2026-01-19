using UnityEngine;

public class HealthbarController : MonoBehaviour
{
    private HeartController[] _hearts;
    [SerializeField] private int testHealthValue; // For debugging purposes

    // For debugging purposes, set testHealthValue in the inspector, right-click the component, and choose this to run the function
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
                // If there's 2 or more health remaining, show this heart as full and get rid of 2hp from the pool
                case >= 2:
                    heart.SetState(2);
                    health -= 2;
                    break;
                // If there's only 1hp left in the pool, then we need this heart to be a half heart, and drain the pool
                case 1:
                    heart.SetState(1);
                    health -= 1;
                    break;
                // Health pool is empty, nothing left to give to this heart, so make it empty
                case >= 0:
                    heart.SetState(0);
                    break;
            }
        }
        
        // Use the first heart in the array to determine the animation progress to be used for the rest
        var animTime = _hearts[0].GetTime();
        // Loop through the remaining hearts, overriding their animation progress to be in sync.
        // When the animation switches, it also restarts, leading to the hearts not being in sync. This fixes that.
        for (int i = 1; i < _hearts.Length; i++)
        {
            _hearts[i].SyncTime(animTime);
        }
    }
}
