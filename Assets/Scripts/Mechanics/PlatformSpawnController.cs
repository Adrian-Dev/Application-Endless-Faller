using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformSpawnController : MonoBehaviour
{
    [SerializeField] List<GameObject> platformList;
    [SerializeField] GameObject parentPlatforms;
    [SerializeField] Material platformHighScoreMaterial;
    [SerializeField] Vector3 initialPosition;

    [SerializeField] public float speed { get; private set; }

    [Tooltip("The higher it is, the quicker platforms will spawn and move")]
    [SerializeField] float spawnRate = 5f;

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

        speed = 1f;
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

        if (timeElapsed > timeNext) // TODO Increase spawn rate and speed of platforms
        {
            timeNext += 1f / spawnRate;
            speed += 0.01f;
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

        if(count == highScoreController.highScore) // About to surpass highScore
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
        timeNext = spawnRate;
        lastIndex = -1;

        foreach (MovingPlatform platform in FindObjectsOfType<MovingPlatform>())
        {
            Destroy(platform.gameObject);
        }

        SpawnPlatform();
    }
}
