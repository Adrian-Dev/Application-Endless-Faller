using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformSpawnController : MonoBehaviour
{
    [SerializeField] List<GameObject> platformList;
    [SerializeField] Vector3 initialPosition;

    System.Random random;

    private void Awake()
    {
        random = new System.Random(); //Chooses a random seed by default. This makes sure every playthrough is almost unique
    }

    public void SpawnPlatform()
    {
        int index = random.Next(0, platformList.Count);

        Instantiate(platformList[index], initialPosition, Quaternion.identity);
    }
}
