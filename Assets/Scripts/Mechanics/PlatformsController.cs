using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Spawning platform system
/// </summary>
public class PlatformsController : MonoBehaviour
{
    [Header("Asset references")]
    [Tooltip("Reference to asset material when player will surpass current high score")]
    [SerializeField] Material _platformHighScoreMaterial;
    [Tooltip("Reference to default asset material")]
    [SerializeField] Material _platformDefaultMaterial;

    Transform _initialPosition;
    PoolPlatformController _poolPlatformController;
    HighScoreController _highScoreController;

    private int _count;
    private bool _surpassed;

    public void InjectDependencies(Transform initialPosition, PoolPlatformController poolPlatformController, HighScoreController highScoreController)
    {
        _initialPosition = initialPosition; 
        _poolPlatformController = poolPlatformController;
        _highScoreController = highScoreController;
    }

    public void SpawnPlatform() 
    {
        MovingPlatform platform = _poolPlatformController.GetRandomPlatform();
        ResetPlatform(platform);

        if (!_surpassed && _count == _highScoreController.HighScore) // About to surpass current high score
        {
            platform.ChangeMaterial(_platformHighScoreMaterial);
            _surpassed = true;
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
        _surpassed = false;

        _poolPlatformController.ReleaseAllPlatforms();
    }

    void ResetPlatform(MovingPlatform platform)
    {
        platform.transform.SetPositionAndRotation(_initialPosition.position, Quaternion.identity);
        platform.ChangeMaterial(_platformDefaultMaterial);
    }
}
