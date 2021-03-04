using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolMovingPlatformController : ScriptableObject
{
    private System.Random _random;

    private List<MovingPlatform> _poolMovingPlatformList;
    private List<int> _platformsAvailable;
    private Dictionary<MovingPlatform, int> _platformsIndexes;

    public void InjectDependencies(List<MovingPlatform> movingPlatformList)
    {
        _random = new System.Random(); //Chooses a random seed by default. This makes sure every playthrough is almost unique
       
        _poolMovingPlatformList = movingPlatformList;

        _platformsAvailable = new List<int>(_poolMovingPlatformList.Count);
        _platformsIndexes = new Dictionary<MovingPlatform, int>(_poolMovingPlatformList.Count);

        for (int i = 0; i < _poolMovingPlatformList.Count; ++i)
        {
            MovingPlatform platform = _poolMovingPlatformList[i];
            _platformsIndexes.Add(platform, i);
            _platformsAvailable.Add(i);
        }
    }

    public MovingPlatform GetRandomPlatform()
    {
        if (!(_platformsAvailable.Count > 0))
        {
            return null;
        }

        int index = _random.Next(0, _platformsAvailable.Count);

        int indexPlatform = _platformsAvailable[index];
        _platformsAvailable[index] = _platformsAvailable[_platformsAvailable.Count - 1]; // Swap with last index element
        _platformsAvailable.RemoveAt(_platformsAvailable.Count - 1); // Remove then las index element (this operation is O(1))

        MovingPlatform movingPlatform = _poolMovingPlatformList[indexPlatform];
        movingPlatform.gameObject.SetActive(true);

        return movingPlatform;
    }

    public void ReleasePlatform(MovingPlatform platform)
    {
        platform.gameObject.SetActive(false);

        int index;
        _platformsIndexes.TryGetValue(platform, out index);

        _platformsAvailable.Add(index);
    }

    public void ReleaseAllPlatforms()
    {
        _platformsAvailable.Clear();

        for (int i = 0; i < _poolMovingPlatformList.Count; ++i)
        {
            _poolMovingPlatformList[i].gameObject.SetActive(false);
            _platformsAvailable.Add(i);
        }
    }
}
