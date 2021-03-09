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
    [SerializeField] CollidedWithTarget _boundaryUpTrigger;
    [SerializeField] CollidedWithTarget _boundaryDownTrigger;
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

    private float _initialSpeed;

    private bool _paused; // switch values for showing menu (game is paused) and closing menu (game is playing) 
    private bool _pausable;

    private bool _isGameLost;

    private float _timeNextPlatform;
    private float _timeElapsed;

    private int _spawnedPlatformsCount;
    private bool _highScoreSurpassed;

    void Awake()
    {
        _pauseController = new PauseController();
        _movingPlatformsList = new List<MovingPlatform>();
    }

    // Entry point 
    void Start() 
    {
        _initialSpeed = _speed;

        // Composition Root pattern

        // Constructor Injection pattern

        _pauseController.InjectDependencies(_world, _menu, _UI);

        LoadPlatformsFromPrefab(_movingPlatformsList);
        _platformsController.InjectDependencies(_movingPlatformsList);

        _spawnPlatformTrigger.SetTargetTag("MovingPlatform");
        _boundaryDownTrigger.SetTargetTag("Player");
        _boundaryUpTrigger.SetTargetTag("Player");

        RestartLevel();
    }

    void Update()
    {
        if (_pausable)
        {
            HandlePauseResume();
        }

        if (!_paused && !_isGameLost)
        {
            if (_boundaryDownTrigger.Collided || _boundaryUpTrigger.Collided)
            {
                StartCoroutine(LoseGame());
            }
            else
            {
                _timeElapsed += Time.deltaTime;
                if (_timeElapsed > _timeNextPlatform)
                {
                    _timeElapsed = 0f;
                    IncreasePlatformsSpeed(0.01f);
                }

                if (_spawnPlatformTrigger.Collided)
                {
                    _spawnPlatformTrigger.ResetCollided();
                    SpawnPlatform();
                }

                HandlePlatformLoop();

                HandlePlayerLoop();
            }

            WriteScoreToText(_currentScoreTextUI);
        }

    }

    void HandlePauseResume()
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
            if (_paused)
            {
                PauseGame();
            }
            else
            {
                ResumeGame();
            }
        }
    }

    void HandlePlatformLoop()
    {
        foreach (MovingPlatform platform in _movingPlatformsList) // Refer to notes about optimization vs use of patterns
        {
            if (platform.gameObject.activeSelf)
            {
                if (platform.CollidedWithTargets[0].Collided) // Collided with player (refer to SetTargetTag)
                {
                    platform.CollidedWithTargets[0].ResetCollided();
                    IncrementScore();
                }
                if (platform.CollidedWithTargets[1].Collided) // Collided with boundary (refer to SetTargetTag)
                {
                    platform.CollidedWithTargets[1].ResetCollided();
                    ReleasePlatform(platform);
                }

                platform.MoveUp(_speed);
            }
        }
    }

    void HandlePlayerLoop()
    {
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            _mainCharacterController.MoveLeft();
        }
        else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            _mainCharacterController.MoveRight();
        }
    }

    void IncrementScore()
    {
        _score++;
    }

    void IncreasePlatformsSpeed(float speed)
    {
        _speed += speed;
    }

    void Initialize()
    {
        _pausable = true;
        _isGameLost = false;
        _score = 0;
        _speed = _initialSpeed;

        _timeElapsed = 0f;
        _timeNextPlatform = 1f / _spawnRateController.SpawnRate;

        _spawnedPlatformsCount = 0;
        _highScoreSurpassed = false;

        WriteScoreToText(_currentScoreTextUI);
        DisplayInfoText(false);

        foreach (MovingPlatform platform in _movingPlatformsList)
        {
            platform.gameObject.SetActive(false);
            platform.CollidedWithTargets[0].ResetCollided();
            platform.CollidedWithTargets[1].ResetCollided();
        }

        _spawnPlatformTrigger.ResetCollided();
        _boundaryUpTrigger.ResetCollided();
        _boundaryDownTrigger.ResetCollided();

        _highScoreController.LoadCurrentHighScore();
        _highScoreController.WriteHighScoreOnText(_highScoreTextMenu);

        _mainCharacterController.Initialize();
        _platformsController.Initialize();
    }


    void SpawnPlatform()
    {
        MovingPlatform platform = _platformsController.GetRandomPlatform();
        ResetPlatform(platform);

        platform.gameObject.SetActive(true);

        if (!_highScoreSurpassed && _spawnedPlatformsCount == _highScoreController.HighScore) // About to surpass current high score
        {
            platform.ChangeMaterial(_platformHighScoreMaterial);
            _highScoreSurpassed = true;
        }

        _spawnedPlatformsCount++;
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

    void LoadPlatformsFromPrefab(List<MovingPlatform> movingPlatformList)
    {
        List<MovingPlatform> prefabsPoolPlatformList = new List<MovingPlatform>(Resources.LoadAll<MovingPlatform>("Platforms"));

        for (int i = 0; i < prefabsPoolPlatformList.Count; ++i)
        {
            MovingPlatform platform = Instantiate(prefabsPoolPlatformList[i]);
            platform.transform.parent = _parentPlatforms;

            platform.CollidedWithTargets[0].SetTargetTag("Player");
            platform.CollidedWithTargets[1].SetTargetTag("Boundary");

            platform.gameObject.SetActive(false);

            movingPlatformList.Add(platform);
        }
    }


    void WriteScoreToText(Text text)
    {
        text.text = _score.ToString();
    }

    void DisplayInfoText(bool value)
    {
        _infoTextMenu.transform.parent.gameObject.SetActive(value); // Inform user of new high score
    }


    IEnumerator LoseGame()
    {
        _isGameLost = true;
        _mainCharacterController.Explode();
        yield return new WaitForSeconds(0.5f);

        _pausable = false;
        PauseGame(true);

        yield return null;
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

        WriteScoreToText(_currentScoreTextMenu);

        if (_score > _highScoreController.HighScore) // Persist new high score
        {
            _highScoreController.SetNewHighScore(_score);
            _highScoreController.WriteHighScoreOnText(_highScoreTextMenu);

            DisplayInfoText(true); // Inform user of new high score
        }

        _pauseController.PauseGame();
    }

    public void ResumeGame() // Also meant to be wired in Scene with CONTINUE button
    {
        _paused = false;
        _pauseController.ResumeGame();
    }

    public void RestartLevel() // Also meant to be wired in Scene with RESTART button
    {
        Initialize();
        SpawnPlatform();
        FadeOff();
        ResumeGame();
    }
}
