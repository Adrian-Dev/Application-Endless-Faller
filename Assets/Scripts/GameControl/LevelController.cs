using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelController : MonoBehaviour
{
    [SerializeField] Image fadeImage;
    [SerializeField] Button continueButton;
    [SerializeField] Button restartButton;
    [SerializeField] Text scoreText;
    [SerializeField] Text highScoreText;
    [SerializeField] Text infoText;

    public int score { get; private set; }

    PauseController pauseController;
    private bool paused; // switch values for showing menu (game is paused) and closing menu (game is playing) 
    private bool m_allow_pause;

    private bool isGameLost;

    MainCharacterController mainCharacterController;
    PlatformSpawnController platformSpawnController;
    HighScoreController highScoreController;

    private void Awake()
    {
        pauseController = GetComponent<PauseController>();
        mainCharacterController = FindObjectOfType<MainCharacterController>();
        platformSpawnController = FindObjectOfType<PlatformSpawnController>();
        highScoreController = FindObjectOfType<HighScoreController>();
    }

    void Start()
    {
        Restart();
    }

    void Update()
    {
        if (m_allow_pause)
        {

            KeyCode pauseMenuKey;
#if UNITY_EDITOR
            pauseMenuKey = KeyCode.M;
#else
        pauseMenuKey = KeyCode.Escape;
#endif

            if (Input.GetKeyUp(pauseMenuKey))
            {
                paused = !paused;
                SetPauseResume();
            }
        }
    }

    void SetPauseResume()
    {
        if (paused)
        {
            PauseGame();
        }
        else
        {
            ResumeGame();
        }
    }

    void FadeOff()
    {
        fadeImage.CrossFadeAlpha(1.0f, 0.0f, true);
        fadeImage.gameObject.SetActive(true);
        fadeImage.CrossFadeAlpha(0.0f, 2.0f, true);
        StartCoroutine(FadeCamera(2.0f));
    }

    IEnumerator FadeCamera(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        fadeImage.gameObject.SetActive(false);

        yield return null;
    }

    /// <summary>
    /// Allows or prevents the game to be paused and resumed each time the user presses the "pause button"
    /// </summary>
    public void AllowPauseGame(bool value)
    {
        m_allow_pause = value;
    }

    public void PauseGame(bool gameLost = false)
    {
        paused = true;
        if (gameLost)
        {
            restartButton.gameObject.SetActive(true);
            continueButton.gameObject.SetActive(false);
        }
        else
        {
            restartButton.gameObject.SetActive(false);
            continueButton.gameObject.SetActive(true);
        }

        scoreText.text = score.ToString();

        if (score > highScoreController.highScore) // Persist new high score
        {
            highScoreText.text = score.ToString();
            highScoreController.SetNewHighScore(score);
            infoText.transform.parent.gameObject.SetActive(true); // Inform user of new high score
        }

        pauseController.PauseGame();
    }

    public void ResumeGame()
    {
        paused = false;
        pauseController.ResumeGame();
    }

    public void IncrementScore()
    {
        if (!isGameLost) // Prevents increasing score if player already lose but the game is still running
        {
            score++;
        }
    }

    public void SetGameLost()
    {
        isGameLost = true;
    }

    public void Restart()
    {
        score = 0;

        mainCharacterController.Restart();
        platformSpawnController.Restart();
        infoText.transform.parent.gameObject.SetActive(false);

        highScoreController.ReadHighScore();

        m_allow_pause = true;

        isGameLost = false;

        FadeOff();
        ResumeGame();
    }
}
