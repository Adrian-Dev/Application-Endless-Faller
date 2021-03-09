using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Platform spawn rate settings
/// </summary>
[CreateAssetMenu(fileName = "Game Settings", menuName = "Game Settings File", order = 1)]
public class GameSettings : ScriptableObject
{
    [Range(1f,50f)]
    [SerializeField] float spawnRate; 

    public float SpawnRate
    {
        get { return spawnRate; }
    }
}
