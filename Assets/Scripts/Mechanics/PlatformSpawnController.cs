using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Spawning platform system
/// </summary>
public class PlatformSpawnController : MonoBehaviour
{
    [Header("Settings")]
    [Tooltip("Initial platforms speed")]
    [SerializeField] float speed;
    [Tooltip("Initial spawning position in the scene")]
    [SerializeField] Vector3 initialPosition;
    [Tooltip("Parent object where platforms will be childed to")]
    [SerializeField] GameObject parentPlatforms;

    [Header("Asset references")]

    [Tooltip("Reference to asset material when player will surpass current high score")]
    [SerializeField] Material platformHighScoreMaterial;
    [Tooltip("Reference to asset platforms to be spawned")]
    [SerializeField] List<GameObject> platformList;


    [Header("Asset Spawn Rate Config")]
    [SerializeField] SpawnRateController spawnRateController;

    public float Speed
    {
        get { return speed; }
    }

    private float initialSpeed;

    private float timeNext;
    private float timeElapsed;

    private System.Random random;
    private int lastIndex;

    HighScoreController highScoreController;
    private int count;

    void Awake()
    {
        random = new System.Random(); //Chooses a random seed by default. This makes sure every playthrough is almost unique

        initialSpeed = speed;
        lastIndex = -1;

        highScoreController = FindObjectOfType<HighScoreController>();
    }

    void Start()
    {
        Restart();
    }

    void Update()
    {
        timeElapsed += Time.deltaTime;

        if (timeElapsed > timeNext)
        {
            timeNext += 1f / spawnRateController.SpawnRate;
            speed += 0.01f; // Increase speed platforms speed
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

        if(count == highScoreController.HighScore) // About to surpass highScore
        {
            foreach(MeshRenderer meshRenderer in platform.GetComponentsInChildren<MeshRenderer>())
            {
                meshRenderer.material = platformHighScoreMaterial;
            }
        }
        count++;

        lastIndex = index;
    }

    public void Restart()
    {
        count = 0;
        speed = initialSpeed;
        timeElapsed = 0f;
        timeNext = 1f / spawnRateController.SpawnRate;
        lastIndex = -1;

        foreach (MovingPlatform platform in FindObjectsOfType<MovingPlatform>())
        {
            Destroy(platform.gameObject);
        }

        SpawnPlatform();
    }
}
