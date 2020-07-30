using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformSpawnController : MonoBehaviour
{
    [SerializeField] List<GameObject> platformList;
    [SerializeField] Vector3 initialPosition;

    private System.Random random;
    private int lastIndex;

    private void Awake()
    {
        random = new System.Random(); //Chooses a random seed by default. This makes sure every playthrough is almost unique
        lastIndex = -1;
    }

    public void SpawnPlatform()
    {
        int index;
        do
        {
            index = random.Next(0, platformList.Count);
        } 
        while (index == lastIndex); // Avoids having the same platform spawned two times in a row

        Instantiate(platformList[index], initialPosition, Quaternion.identity);

        lastIndex = index;
    }
}
