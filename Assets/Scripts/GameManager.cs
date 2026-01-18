using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int scoreMult = 10;
    public static GameManager Instance { get; private set; }
    public int Score { get; private set; }
    public int Difficulty { get; private set; } = 1;

    [Header("SFX Config")]
    [SerializeField] private AudioClip loopingMusic;
    [SerializeField] private AudioClip enemyDeathSound;
    [SerializeField] private float enemyDeathPitchVariation;
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource musicSource;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        PlayMusic(loopingMusic);
    }

    private void PlayMusic(AudioClip clip)
    {
        musicSource.clip = clip;
        musicSource.loop = true;
        musicSource.Play();
    }

    private void PlaySfx(AudioClip clip, float pitchOffset)
    {
        var basePitch = sfxSource.pitch;
        sfxSource.pitch += pitchOffset;
        sfxSource.PlayOneShot(clip);
        sfxSource.pitch = basePitch;
    }

    public void AddScore(int amount)
    {
        Score += scoreMult * amount;
    }

    public void EnemyDeathSound()
    {
        PlaySfx(enemyDeathSound, Random.Range(-enemyDeathPitchVariation, enemyDeathPitchVariation));
    }

    // For testing
    // void Update()
    // {
    //     if (Input.GetMouseButtonDown(0))
    //     {
    //         EnemyDeathSound();
    //     }
    // }
}