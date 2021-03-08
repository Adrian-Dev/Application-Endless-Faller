using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controls the game flow and application state
/// </summary>
public class LevelController : MonoBehaviour // TODO finished tooltips properly and description of all scripts
{
    [Header("References to inject on pause controller")]
    [SerializeField] GameObject _world;
    [SerializeField] GameObject _menu;
    [SerializeField] GameObject _UI;

    [Header("Platforms settings")]
    [Tooltip("Initial platforms speed")]
    [SerializeField] float _speed;
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

    [Header("Reference to Depencies")]
    [SerializeField] CollidedWithTarget _boundaryTriggerUp;
    [SerializeField] CollidedWithTarget _boundaryTriggerDown;
    [SerializeField] CollidedWithTarget _spawnPlatformTrigger;
    [SerializeField] PlatformsController _platformsController;
    [SerializeField] MainCharacterController _mainCharacterController;
    [SerializeField] HighScoreController _highScoreController;
    [Tooltip("Asset Spawn Rate Config")]
    [SerializeField] SpawnRateController _spawnRateController;

    [Header("Asset references")]
    [Tooltip("Reference to asset material when player will surpass current high score")]
    [SerializeField] Material _platformHighScoreMaterial;
    [Tooltip("Reference to default asset material")]
    [SerializeField] Material _platformDefaultMaterial;

    PauseController _pauseController;

    List<MovingPlatform> _movingPlatformsList;

    public int Score { get { return _score; } }
    int _score;

    public float PlatformsSpeed
    {
        get { return _speed; }
    }
    private float _initialSpeed;

    private bool _paused; // switch values for showing menu (game is paused) and closing menu (game is playing) 
    private bool _m_allow_pause;

    private bool _isGameLost;

    private float _timeNextPlatform;
    private float _timeElapsed;

    private int _count;
    private bool _surpassed;

    void Awake()
    {
        // Entry point - Composition Root pattern

        // Constructor Injection pattern

        _pauseController = new PauseController();
        _pauseController.InjectDependencies(_world, _menu, _UI);

        _movingPlatformsList = new List<MovingPlatform>();
        LoadPlatformsFromPrefab(_movingPlatformsList);
        _platformsController.InjectDependencies(_movingPlatformsList);

        // Set Tags here...
        _spawnPlatformTrigger.SetTag("MovingPlatform");
        _boundaryTriggerDown.SetTag("Player");
        _boundaryTriggerUp.SetTag("Player");
    }

    void Start()
    {
        _initialSpeed = _speed;

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

        if (!_paused && !_isGameLost)
        {
            if (_boundaryTriggerDown.Triggered || _boundaryTriggerUp.Triggered)
            {
                StartCoroutine(LoseGame());
            }
            else
            {
                if (_spawnPlatformTrigger.Triggered)
                {
                    SpawnPlatform();
                    _spawnPlatformTrigger.Triggered = false;
                }

                if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
                {
                    _mainCharacterController.MoveLeft();
                }
                else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
                {
                    _mainCharacterController.MoveRight();
                }

                foreach (MovingPlatform platform in _movingPlatformsList) // Refer to notes about optimization vs use of patterns
                {
                    if (platform.gameObject.activeSelf)
                    {
                        platform.MoveUp(_speed);

                        if (platform.CollidedWithPlayer)
                        {
                            IncrementScore();
                            platform.CollidedWithPlayer = false;
                        }
                        if (platform.CollidedWitBoundary)
                        {
                            ReleasePlatform(platform);
                            platform.CollidedWitBoundary = false;
                        }
                    }
                }

                _timeElapsed += Time.deltaTime;
                if (_timeElapsed > _timeNextPlatform)
                {
                    _timeElapsed = 0f;
                    IncreasePlatformsSpeed(0.01f);
                }
            }

            _currentScoreTextUI.text = Score.ToString();
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
    void AllowPauseGame(bool value)
    {
        _m_allow_pause = value;
    }

    void PauseGame(bool gameLost = false)
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

    void IncrementScore()
    {
        if (!_isGameLost) // Prevents increasing score if player already lost but the game is still running
        {
            _score++;
        }
    }

    void IncreasePlatformsSpeed(float speed)
    {
        _speed += speed;
    }

    void SetGameLost()
    {
        _isGameLost = true;
    }

    void Initialize()
    {
        _m_allow_pause = true;
        _isGameLost = false;
        _score = 0;
        _speed = _initialSpeed;

        _timeElapsed = 0f;
        _timeNextPlatform = 1f / _spawnRateController.SpawnRate;

        _count = 0;
        _surpassed = false;

        foreach (MovingPlatform platform in _movingPlatformsList) // Refer to notes about optimization vs use of patterns
        {
            platform.gameObject.SetActive(false);
        }

        _highScoreController.LoadCurrentHighScore();
        _highScoreController.WriteHighScoreOnText(_highScoreTextMenu);

        _mainCharacterController.Initialize();
        _platformsController.Initialize();

        _spawnPlatformTrigger.ResetTrigger();
        _boundaryTriggerUp.ResetTrigger();
        _boundaryTriggerDown.ResetTrigger();

        SpawnPlatform();

        _currentScoreTextUI.text = Score.ToString();
        _infoTextMenu.transform.parent.gameObject.SetActive(false); // work around to get a better solution rather than doing this in this line
    }

    void LoadPlatformsFromPrefab(List<MovingPlatform> movingPlatformList)
    {
        List<MovingPlatform> prefabsPoolPlatformList = new List<MovingPlatform>(Resources.LoadAll<MovingPlatform>("Platforms"));

        for (int i = 0; i < prefabsPoolPlatformList.Count; ++i)
        {
            MovingPlatform platform = Instantiate(prefabsPoolPlatformList[i]);
            platform.transform.parent = _parentPlatforms;
            platform.gameObject.SetActive(false);

            //TODO set platform TAGS

            movingPlatformList.Add(platform);
        }
    }

    void SpawnPlatform()
    {
        MovingPlatform platform = _platformsController.GetRandomPlatform();
        ResetPlatform(platform);

        platform.gameObject.SetActive(true);

        if (!_surpassed && _count == _highScoreController.HighScore) // About to surpass current high score // latforms controller should only care about spawn and release, according its inner implemtation
        {
            platform.ChangeMaterial(_platformHighScoreMaterial);
            _surpassed = true;
        }

        _count++;
    }

    void ReleasePlatform(MovingPlatform platform)
    {
        _platformsController.ReleasePlatform(platform);
        platform.gameObject.SetActive(false);
    }

    void ResetPlatform(MovingPlatform platform)
    {
        platform.transform.SetPositionAndRotation(_initialPlatformPosition.position, Quaternion.identity);
        platform.ChangeMaterial(_platformDefaultMaterial);
    }


    IEnumerator LoseGame()
    {
        SetGameLost();
        _mainCharacterController.Explode();
        yield return new WaitForSeconds(0.5f);
        AllowPauseGame(false);
        PauseGame(true);
        yield return null;
    }
}
