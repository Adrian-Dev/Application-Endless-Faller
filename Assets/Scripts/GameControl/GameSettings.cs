using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Platform spawn rate settings
/// </summary>
[CreateAssetMenu(fileName = "Game Settings", menuName = "Game Settings File", order = 1)]
public class GameSettings : ScriptableObject
{
    [Range(1f, 50f)]
    [SerializeField] float _spawnRate;
    [Range(1f, 13f)]
    [SerializeField] float _initialPlayerSpeed;
    [Range(1f, 7f)]
    [SerializeField] float _initialPlatformsSpeed;

    public float SpawnRate
    {
        get { return _spawnRate; }
    }
    public float InitialPlayerSpeed
    {
        get { return _initialPlayerSpeed; }
    }
    public float InitialPlatformsSpeed
    {
        get { return _initialPlatformsSpeed; }
    }
}
