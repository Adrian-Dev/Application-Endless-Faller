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

    [Header("Asset references")]
    [Tooltip("Reference to asset material when player will surpass current high score")]
    [SerializeField] Material _platformHighScoreMaterial;
    [Tooltip("Reference to default asset material")]
    [SerializeField] Material _platformDefaultMaterial;

    Transform _initialPosition;
    PoolPlatformController _poolPlatformController;
    HighScoreController _highScoreController;

    public float Speed
    {
        get { return _speed; }
    }

    private float _initialSpeed;
    private int _currentHighScore;
    private int _count;

    public void InjectDependencies(Transform initialPosition, PoolPlatformController poolPlatformController, HighScoreController highScoreController)
    {
        _initialPosition = initialPosition; 
        _poolPlatformController = poolPlatformController;
        _highScoreController = highScoreController;
    }

    void Awake()
    {
        _initialSpeed = _speed;
    }

    public void IncreasePlatformsSpeed(float speed)
    {
        _speed += speed;
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
