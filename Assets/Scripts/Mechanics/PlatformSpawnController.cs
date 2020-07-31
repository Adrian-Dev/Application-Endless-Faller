using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformSpawnController : MonoBehaviour
{
    [SerializeField] List<GameObject> platformList;
    [SerializeField] GameObject parentPlatforms;
    [SerializeField] Vector3 initialPosition;
    [SerializeField] public float speed { get; private set; }

    [SerializeField] float spawnRate = 2f;

    private float initialSpeed;

    private float timeNext;
    private float timeElapsed;

    private System.Random random;
    private int lastIndex;

    void Awake()
    {
        speed = 1f;
        initialSpeed = speed;
        random = new System.Random(); //Chooses a random seed by default. This makes sure every playthrough is almost unique
        lastIndex = -1;
    }

    void Start()
    {
        Restart();
    }

    void Update()
    {
        timeElapsed += Time.deltaTime;

        if (timeElapsed > timeNext) // TODO Increase spawn rate and speed of platforms
        {
            timeNext += spawnRate;
            speed += 0.1f;
        }
    }

    public void SpawnPlatform()
    {
        int index;
        do
        {
            index = random.Next(0, platformList.Count);
        } 
        while (index == lastIndex); // Avoids having the same platform spawned two times in a row

        GameObject platform = Instantiate(platformList[index], initialPosition, Quaternion.identity);
        platform.transform.parent = parentPlatforms.transform;

        lastIndex = index;
    }

    public void Restart()
    {
        speed = initialSpeed;
        timeElapsed = 0f;
        timeNext = spawnRate;

        foreach (MovingPlatform platform in FindObjectsOfType<MovingPlatform>())
        {
            Destroy(platform.gameObject);
        }
        SpawnPlatform();
    }
}
