using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controls the game flow and application state
/// </summary>
public class LevelController : MonoBehaviour
{
    [Tooltip("Parent object where platforms will be childed to")]
    [SerializeField] Transform _parentPlatforms;

    [Header("References to Menu elements")]
    [SerializeField] Button continueButton;
    [SerializeField] Button restartButton;
    [SerializeField] Text scoreText;
    [SerializeField] Text highScoreText;
    [SerializeField] Text infoText;

    [Header("Reference to Fade Image element")]
    [SerializeField] Image fadeImage;

    [Header("Reference to Depencies to be Injected")]
    [SerializeField] BoundaryController _boundaryControllerUp;
    [SerializeField] BoundaryController _boundaryControllerDown;
    [SerializeField] NextPlatformTrigger _nextPlatformTrigger;
    [SerializeField] PlatformsController _platformsController;
    [SerializeField] MainCharacterController _mainCharacterController;
    [SerializeField] HighScoreController _highScoreController;

    [SerializeField] PauseController _pauseController;

    [Header("Asset Spawn Rate Config")]
    [SerializeField] SpawnRateController _spawnRateController;

    PoolPlatformController _poolPlatformController;
    List<MovingPlatform> _movingPlatformList;

    public int Score { get { return _score; } }
    int _score;

    private bool _paused; // switch values for showing menu (game is paused) and closing menu (game is playing) 
    private bool _m_allow_pause;

    private bool _isGameLost;

    private void Awake()
    {
        _movingPlatformList = new List<MovingPlatform>();
        LoadPlatformsFromPrefab(_movingPlatformList);

        _poolPlatformController = ScriptableObject.CreateInstance<PoolPlatformController>();
        _poolPlatformController.InjectDependencies(_movingPlatformList);

        _boundaryControllerUp.InjectDependencies(this, _mainCharacterController);
        _boundaryControllerDown.InjectDependencies(this, _mainCharacterController);
        _nextPlatformTrigger.InjectDependencies(_platformsController);
        _platformsController.InjectDependencies(_poolPlatformController, _spawnRateController, _highScoreController);

    }

    void Start()
    {
        RestartGame();
    }

    void Update()
    {
        if (_m_allow_pause)
        {
            KeyCode pauseMenuKey;
#if UNITY_EDITOR
            pauseMenuKey = KeyCode.M;
#else
            pauseMenuKey = KeyCode.Escape;
#endif
            if (Input.GetKeyUp(pauseMenuKey))
            {
                _paused = !_paused;
                SetPauseResume();
            }
        }

        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            _mainCharacterController.MoveLeft();
        }
        else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            _mainCharacterController.MoveRight();
        }

    }

    void SetPauseResume()
    {
        if (_paused)
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
        _m_allow_pause = value;
    }

    public void PauseGame(bool gameLost = false)
    {
        _paused = true;
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

        scoreText.text = _score.ToString();

        if (_score > _highScoreController.HighScore) // Persist new high score
        {
            highScoreText.text = _score.ToString();
            _highScoreController.SetNewHighScore(_score);
            infoText.transform.parent.gameObject.SetActive(true); // Inform user of new high score
        }

        _pauseController.PauseGame();
    }

    public void ResumeGame() // Also meant to be wired in Scene with CONTINUE button
    {
        _paused = false;
        _pauseController.ResumeGame();
    }

    public void RestartGame() // Also meant to be wired in Scene with RESTART button
    {
        Initialize();
        FadeOff();
        ResumeGame();
    }

    public void IncrementScore()
    {
        if (!_isGameLost) // Prevents increasing score if player already lost but the game is still running
        {
            _score++;
        }
    }

    public void SetGameLost()
    {
        _isGameLost = true;
    }

    public void Initialize()
    {
        _m_allow_pause = true;
        _isGameLost = false;
        _score = 0;

        _mainCharacterController.Initialize();
        _platformsController.Initialize();
        infoText.transform.parent.gameObject.SetActive(false); // work around to get a better solution rather than doing this in this line

        _highScoreController.ReadHighScore();
    }


    void LoadPlatformsFromPrefab(List<MovingPlatform> movingPlatformList)
    {
        List<MovingPlatform> prefabsPoolPlatformList = new List<MovingPlatform>(Resources.LoadAll<MovingPlatform>("Platforms"));

        for (int i = 0; i < prefabsPoolPlatformList.Count; ++i)
        {
            MovingPlatform platform = Instantiate(prefabsPoolPlatformList[i]);
            platform.InjectDependencies(this, _platformsController);
            platform.transform.parent = _parentPlatforms;
            platform.gameObject.SetActive(false);

            movingPlatformList.Add(platform);
        }
    }
}
