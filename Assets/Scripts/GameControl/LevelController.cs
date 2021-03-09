using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controls the game flow and level state
/// </summary>
public class LevelController : MonoBehaviour
{
    [Header("References to inject on pause controller")]
    [SerializeField] GameObject _world;
    [SerializeField] GameObject _menu;
    [SerializeField] GameObject _UI;

    [Header("Player settings")]
    [Tooltip("Initial player speed")]
    [SerializeField] float _playerSpeed;

    [Header("Platforms settings")]
    [Tooltip("Initial platforms speed")]
    [SerializeField] float _platformsSpeed;
    [Tooltip("Initial spawning platform position in the scene")]
    [SerializeField] Transform _initialPlatformPosition;
    [Tooltip("Parent object where platforms will be childed to")]
    [SerializeField] Transform _parentPlatforms;

    [Header("References to Menu elements")]
    [SerializeField] Button _continueButton;
    [SerializeField] Button _restartButton;
    [SerializeField] Text _currentScoreTextMenu;
    [SerializeField] Text _highScoreTextMenu;
    [Tooltip("Message displayed when poping the Menu up")]
    [SerializeField] Text _infoTextMenu;

    [Header("References to UI elements")]
    [SerializeField] Text _currentScoreTextUI;

    [Header("Reference to Fade Image element")]
    [SerializeField] Image fadeImage;

    [Header("Reference to Depencies")]
    [SerializeField] CollidedWithTarget _boundaryUpTrigger;
    [SerializeField] CollidedWithTarget _boundaryDownTrigger;
    [SerializeField] CollidedWithTarget _spawnPlatformTrigger;
    [SerializeField] MovingPlatformProvider _movingPlatformProvider;
    [SerializeField] MainCharacter _mainCharacter;
    [SerializeField] HighScoreController _highScoreController;

    [Header("References to assets")]
    [Tooltip("Game settings file")]
    [SerializeField] GameSettings _gameSettings;
    [Tooltip("Material when surpassing current high score")]
    [SerializeField] Material _platformHighScoreMaterial;
    [Tooltip("Default material")]
    [SerializeField] Material _platformDefaultMaterial;

    PauseController _pauseController;

    List<MovingPlatform> _movingPlatformsList;

    public int Score { get { return _score; } }
    int _score;

    private float _initialPlatformSpeed;
    private float _initialPlayerSpeed;

    private bool _paused;
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
        _initialPlatformSpeed = _platformsSpeed;
        _initialPlayerSpeed = _playerSpeed;
        // Composition Root 

        _pauseController.InjectDependencies(_world, _menu, _UI);

        LoadPlatformsFromPrefab(_movingPlatformsList);
        _movingPlatformProvider.InjectDependencies(_movingPlatformsList);

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

                platform.MoveUp(_platformsSpeed);
            }
        }
    }

    void HandlePlayerLoop()
    {
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            _mainCharacter.Move(Vector3.left * _playerSpeed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            _mainCharacter.Move(Vector3.right * _playerSpeed * Time.deltaTime);
        }
    }

    void IncrementScore()
    {
        _score++;
    }

    void IncreasePlatformsSpeed(float speed)
    {
        _platformsSpeed += speed;
    }


    void SpawnPlatform()
    {
        MovingPlatform platform = _movingPlatformProvider.GetRandomPlatform();
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
        _movingPlatformProvider.ReleasePlatform(platform);
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
        _infoTextMenu.transform.parent.gameObject.SetActive(value);
    }


    IEnumerator LoseGame()
    {
        _isGameLost = true;
        _mainCharacter.Explode();
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


    void Initialize()
    {
        _pausable = true;
        _isGameLost = false;
        _score = 0;
        _platformsSpeed = _initialPlatformSpeed;
        _playerSpeed = _initialPlayerSpeed;

        _timeElapsed = 0f;
        _timeNextPlatform = 1f / _gameSettings.SpawnRate;

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

        _mainCharacter.Initialize();
        _movingPlatformProvider.Initialize();
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
