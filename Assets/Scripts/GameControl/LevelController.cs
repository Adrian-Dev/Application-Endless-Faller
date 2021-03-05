using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controls the game flow and application state
/// </summary>
public class LevelController : MonoBehaviour
{
    [Header("References to inject on pause controller")]
    [SerializeField] GameObject _world;
    [SerializeField] GameObject _menu;
    [SerializeField] GameObject _UI;

    [Header("Platforms settings")]
    [Tooltip("Initial spawning platform position in the scene")]
    [SerializeField] Transform _initialPlatformPosition;
    [Tooltip("Parent object where platforms will be childed to")]
    [SerializeField] Transform _parentPlatforms;

    [Header("References to Menu elements")]
    [SerializeField] Button _continueButton;
    [SerializeField] Button _restartButton;
    [SerializeField] Text _currentScoreTextMenu;
    [Tooltip("Refence to high score in the Menu UI")]
    [SerializeField] Text _highScoreTextMenu;
    [SerializeField] Text _infoTextMenu;

    [Header("References to UI elements")]
    [Tooltip("Reference to players score UI element")]
    [SerializeField] Text _currentScoreTextUI;

    [Header("Reference to Fade Image element")]
    [SerializeField] Image fadeImage;

    [Header("Reference to Depencies to be Injected")]
    [SerializeField] BoundaryController _boundaryControllerUp;
    [SerializeField] BoundaryController _boundaryControllerDown;
    [SerializeField] NextPlatformTrigger _nextPlatformTrigger;
    [SerializeField] PlatformsController _platformsController;
    [SerializeField] MainCharacterController _mainCharacterController;
    [SerializeField] HighScoreController _highScoreController;

    [Header("Asset Spawn Rate Config")]
    [SerializeField] SpawnRateController _spawnRateController;

    PauseController _pauseController;

    PoolPlatformController _poolPlatformController;
    List<MovingPlatform> _movingPlatformList;

    public int Score { get { return _score; } }
    int _score;

    private bool _paused; // switch values for showing menu (game is paused) and closing menu (game is playing) 
    private bool _m_allow_pause;

    private bool _isGameLost;

    private float _timeNextPlatform;
    private float _timeElapsed;

    private void Awake()
    {
        // Entry point - Composition Root

        _movingPlatformList = new List<MovingPlatform>();
        LoadPlatformsFromPrefab(_movingPlatformList);

        _pauseController = new PauseController();
        _pauseController.InjectDependencies(_world, _menu, _UI);

        _poolPlatformController = ScriptableObject.CreateInstance<PoolPlatformController>();
        _poolPlatformController.InjectDependencies(_movingPlatformList);

        _boundaryControllerUp.InjectDependencies(this, _mainCharacterController);
        _boundaryControllerDown.InjectDependencies(this, _mainCharacterController);
        _nextPlatformTrigger.InjectDependencies(_platformsController);
        _platformsController.InjectDependencies(_initialPlatformPosition, _poolPlatformController, _highScoreController);
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

        _currentScoreTextUI.text = Score.ToString();


        _timeElapsed += Time.deltaTime;
        if (_timeElapsed > _timeNextPlatform)
        {
            _timeElapsed = 0f;
            _platformsController.IncreasePlatformsSpeed(0.01f);
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
            _restartButton.gameObject.SetActive(true);
            _continueButton.gameObject.SetActive(false);
        }
        else
        {
            _restartButton.gameObject.SetActive(false);
            _continueButton.gameObject.SetActive(true);
        }

        _currentScoreTextMenu.text = _score.ToString();

        if (_score > _highScoreController.HighScore) // Persist new high score
        {
            _highScoreController.SetNewHighScore(_score);
            _highScoreController.WriteHighScoreOnText(_highScoreTextMenu);

            _infoTextMenu.transform.parent.gameObject.SetActive(true); // Inform user of new high score
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

        _timeElapsed = 0f;
        _timeNextPlatform = 1f / _spawnRateController.SpawnRate;

        _mainCharacterController.Initialize();
        _platformsController.Initialize();
        _infoTextMenu.transform.parent.gameObject.SetActive(false); // work around to get a better solution rather than doing this in this line

        _highScoreController.LoadCurrentHighScore();
        _highScoreController.WriteHighScoreOnText(_highScoreTextMenu);
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
