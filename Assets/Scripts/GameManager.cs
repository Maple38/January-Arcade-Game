using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int scoreMult = 10;
    public static GameManager Instance { get; private set; }
    public int Score { get; private set; }
    public int Difficulty { get; private set; } = 1;

    private void Awake()
    {
        Instance = this;
    }

    public void AddScore(int amount)
    {
        Score += scoreMult * amount;
    }
}