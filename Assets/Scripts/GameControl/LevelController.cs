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
    [SerializeField] Text scoreMaxText;

    public int Score { get; private set; }

    PauseController pauseController;
    private bool paused; // switch values for showing menu (game is paused) and closing menu (game is playing) 
    private bool m_allow_pause;

    MainCharacterController mainCharacterController;
    PlatformSpawnController platformSpawnController;

    private void Awake()
    {
        pauseController = GetComponent<PauseController>();
        mainCharacterController = FindObjectOfType<MainCharacterController>();
        platformSpawnController = FindObjectOfType<PlatformSpawnController>();
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
    /// Allows the game to be paused and resumed each time the user presses the "pause button"
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
            scoreText.transform.parent.gameObject.SetActive(true);
            scoreMaxText.transform.parent.gameObject.SetActive(true);
            scoreText.text = Score.ToString();
        }
        else
        {
            restartButton.gameObject.SetActive(false);
            continueButton.gameObject.SetActive(true);
            scoreText.transform.parent.gameObject.SetActive(false);
            scoreMaxText.transform.parent.gameObject.SetActive(false);

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
        Score++;
    }

    public void Restart()
    {
        Score = 0;

        mainCharacterController.Restart();
        platformSpawnController.Restart();

        m_allow_pause = true;

        FadeOff();
        ResumeGame();
    }
}
