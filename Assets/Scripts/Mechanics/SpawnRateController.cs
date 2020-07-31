using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Spawn Rate Config", menuName = "Spawn Platform/Spawn Rate", order = 1)]
public class SpawnRateController : ScriptableObject
{
    [Range(1f,50f)]
    [SerializeField] float spawnRate; 

    public float SpawnRate
    {
        get { return spawnRate; }
    }
}
