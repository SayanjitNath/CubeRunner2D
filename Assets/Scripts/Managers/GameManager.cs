using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private Transform ghostPlayerTransform;
    [SerializeField] private float xOffset = 1f;
    [SerializeField] private float bgScrollSpeed = 0.3f;
    [SerializeField] private float interactableMoveSpeed = 5f;
    [SerializeField] private BgScrollSpeedController scrollSpeedController;
    [SerializeField] private InteractableController interactableController;
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private PLayerHealthPointUI playerHealthPointUI;
    [SerializeField] private Button jumpButton;
    [SerializeField] private GameObject gameOverPanel;

    [SerializeField] private AudioClip bgMusic;
    [SerializeField] private AudioClip gameOverCLip;

    private int playerScore;
    private int playerHighScore;

    private bool isGameOver = false;



    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        Application.targetFrameRate = 60;
    }

    public void StartGame()
    {
        SoundManager.instance.PlayBackgroundMusic(bgMusic, true);
        ResetEverything();
        PlayGame();
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
        System.Diagnostics.Process.GetCurrentProcess().Kill();
#endif
    }

    private void ResetEverything()
    {
        isGameOver = false;
        ResetScore();
        playerHighScore = PlayerPrefs.GetInt("HighScore");


        ResetGhostPlayerTransform();

        playerHealthPointUI.InitialSetUp();
        playerHealth.InitialSetUp();

        scrollSpeedController.SetScrollSpeed(bgScrollSpeed, true);
        interactableController.SetSpeed(interactableMoveSpeed, true);
    }


    public void UpdateScore(int scoreToAdd)
    {
        playerScore += scoreToAdd;
        UpdateHighScore();
    }

    private void UpdateHighScore()
    {
        playerHighScore = Mathf.Max(playerHighScore, playerScore);
        PlayerPrefs.SetInt("HighScore", playerHighScore);
    }

    private void ResetScore() => playerScore = 0;

    private void ResetGhostPlayerTransform()
    {
        float cameraHeight = Camera.main.orthographicSize * 2;
        float cameraWidth = cameraHeight * Camera.main.aspect;

        ghostPlayerTransform.position = new Vector2(xOffset - cameraWidth * 0.5f, ghostPlayerTransform.position.y);
    }

    public int GetCurrentScore() => playerScore;

    public int GetHighScore() => playerHighScore;

    public bool IsGameOver() => isGameOver;


    public void PauseGame()
    {
        jumpButton.enabled = false;
        playerController.LockYPosition();  
        scrollSpeedController.StopScrolling();
        interactableController.StopSpawning(0f);
    }

    public void PlayGame()
    {
        playerController.UnlockYPosition();
        scrollSpeedController.ResumeScrolling();
        interactableController.StartSpawning(interactableMoveSpeed);
        jumpButton.enabled = true;
    }

    public void GameOver()
    {
        SoundManager.instance.StopBackgroundMusic();
        SoundManager.instance.PlaySfxSound(gameOverCLip,false);
        isGameOver = true;
        gameOverPanel.transform.localScale = Vector3.zero;
        gameOverPanel.SetActive(true);
        gameOverPanel.transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.Linear);
    }

    public void MainMenu()
    {
        PauseGame();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
