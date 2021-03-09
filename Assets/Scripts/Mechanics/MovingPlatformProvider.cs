using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Provides moving platforms to a consumer 
/// </summary>
public class MovingPlatformProvider : MonoBehaviour
{
    PoolingSystem<MovingPlatform> _poolPlatforms;

    public void InjectDependencies(List<MovingPlatform> movingPlatformsList)
    {
        _poolPlatforms = new PoolingSystem<MovingPlatform> (movingPlatformsList);
    }

    public MovingPlatform GetRandomPlatform()
    {
        MovingPlatform platform = _poolPlatforms.GetRandomItem(); 

        return platform;
    }

    public void ReleasePlatform(MovingPlatform platform)
    {
        _poolPlatforms.ReleaseItem(platform);
    }

    public void Initialize()
    {
        _poolPlatforms.ReleaseItems();
    }
}
