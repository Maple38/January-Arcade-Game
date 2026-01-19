using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int scoreMult = 10;

    [Header("SFX Config")]
    [SerializeField] private AudioClip loopingMusic;
    [SerializeField] private AudioClip enemyDeathSound;
    [SerializeField] private float enemyDeathPitchVariation;
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private string scorePrefix;
    [SerializeField] private GameObject loseText;
    [SerializeField] private GameObject player;
    public float globalScrollSpeed;
    public static GameManager Instance { get; private set; }
    public int Score { get; private set; }
    public int Difficulty { get; private set; } = 1;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        loseText.SetActive(false);
        PlayMusic(loopingMusic);
    }

    private void LateUpdate()
    {
        scoreText.text = scorePrefix + Score.ToString("D5");
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

    public void TriggerLoss()
    {
        loseText.SetActive(true);
        musicSource.Stop();
        StartCoroutine(RestartAfterDelay(5f));
    }

    IEnumerator RestartAfterDelay(float time)
    {
        yield return new WaitForSeconds(time);
        SceneManager.LoadScene(0);
    }

    public Vector2 FetchPlayerPos()
    {
        return player.transform.position;
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