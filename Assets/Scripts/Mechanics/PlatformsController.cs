using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Spawning platform system. Decorator pattern?? This class currently wraps the implementation of the pooling system. 
/// The pooling system could be changed for other system. For example, another implementation, easier but less efficient, 
/// would be creating a copy of the selected random platform, returning it to the client, and, when calling release, destroying the platform,
/// keeping always the initial platform list safe
/// </summary>
public class PlatformsController : MonoBehaviour
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
