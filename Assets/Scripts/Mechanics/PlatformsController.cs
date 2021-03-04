using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Spawning platform system
/// </summary>
public class PlatformsController : MonoBehaviour
{
    [Header("Settings")]
    [Tooltip("Initial platforms speed")]
    [SerializeField] float _speed;
    [Tooltip("Initial spawning platform position in the scene")]
    [SerializeField] Transform _initialPosition;

    [Header("Asset references")]
    [Tooltip("Reference to asset material when player will surpass current high score")]
    [SerializeField] Material _platformHighScoreMaterial;
    [Tooltip("Reference to default asset material")]
    [SerializeField] Material _platformDefaultMaterial;

    PoolPlatformController _poolPlatformController;
    SpawnRateController _spawnRateController;
    HighScoreController _highScoreController;

    public float Speed
    {
        get { return _speed; }
    }

    private float _initialSpeed;
    private float _timeNext;
    private float _timeElapsed;
    private int _currentHighScore;
    private int _count;

    public void InjectDependencies(PoolPlatformController poolPlatformController, SpawnRateController spawnRateController, HighScoreController highScoreController)
    {
        _poolPlatformController = poolPlatformController;
        _spawnRateController = spawnRateController;
        _highScoreController = highScoreController;
    }

    void Awake()
    {
        _initialSpeed = _speed;
    }

    void Update()
    {
        _timeElapsed += Time.deltaTime;

        if (_timeElapsed > _timeNext)
        {
            _timeElapsed = 0f;
            _speed += 0.01f; // Increase platforms speed
        }
    }

    public void SpawnPlatform()
    {
        MovingPlatform platform = _poolPlatformController.GetRandomPlatform();
        ResetPlatform(platform);

        if (_count == _currentHighScore) // About to surpass current high score
        {
            platform.ChangeMaterial(_platformHighScoreMaterial);
        }

        _count++;
    }

    public void ReleasePlatform(MovingPlatform platform)
    {
        _poolPlatformController.ReleasePlatform(platform);
    }

    public void Initialize()
    {
        _count = 0;
        _speed = _initialSpeed;
        _timeElapsed = 0f;
        _timeNext = 1f / _spawnRateController.SpawnRate;

        _currentHighScore = _highScoreController.HighScore;

        _poolPlatformController.ReleaseAllPlatforms();

        SpawnPlatform();
    }

    void ResetPlatform(MovingPlatform platform)
    {
        platform.transform.SetPositionAndRotation(_initialPosition.position, Quaternion.identity);
        platform.ChangeMaterial(_platformDefaultMaterial);
    }
}
