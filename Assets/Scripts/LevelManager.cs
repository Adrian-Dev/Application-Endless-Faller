using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> Manages the state of the level </summary>
public class LevelManager : MonoBehaviour
{
    public int Score { get; private set; }

    [SerializeField] float timeForIncreasingSpawnRate = 5f;

    GameObject player;
    MainCharacter mainCharacter;
    PlatformSpawnController platformSpawnController;

    private float timeNext;
    private float timeElapsed;

    private void Awake()
    {
        player = GameObject.FindWithTag("Player");
        mainCharacter = player.GetComponent<MainCharacter>();
        platformSpawnController = FindObjectOfType<PlatformSpawnController>();
    }
    private void Start()
    {
        Reset();
    }
    void Update()
    {
        timeElapsed += Time.deltaTime;

        if (timeElapsed > timeNext) // TODO Increase spawn rate and speed of platforms
        {
            timeNext += timeForIncreasingSpawnRate;

            platformSpawnController.SpawnPlatform();
        }
    }

    public void IncrementScore()
    {
        Score++;
    }

    public void Reset()
    {
        Score = 0;

        timeElapsed = 0f;
        timeNext = timeForIncreasingSpawnRate;

        player.transform.position = mainCharacter.initialTransform;

        foreach(MovingPlatform platform in FindObjectsOfType<MovingPlatform>())
        {
            Destroy(platform.gameObject);
        }

        platformSpawnController.SpawnPlatform();
    }
}
