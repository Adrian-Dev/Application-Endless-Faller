using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Spawning platform system
/// </summary>
public class ActivePlatformsController : MonoBehaviour
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
    [Tooltip("Reference to default asset material")]
    [SerializeField] Material platformDefaultMaterial;

    [Header("Asset Spawn Rate Config")]
    [SerializeField] SpawnRateController spawnRateController;

    PoolPlatformManager poolPlatforms;

    public float Speed
    {
        get { return speed; }
    }

    private float initialSpeed;

    private float timeNext;
    private float timeElapsed;

    HighScoreController highScoreController;
    private int currentHighScore;
    private int count;


    void Awake()
    {
        initialSpeed = speed;

        highScoreController = FindObjectOfType<HighScoreController>();

        poolPlatforms = new PoolPlatformManager(initialPosition, platformDefaultMaterial, parentPlatforms.transform);
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
            speed += 0.01f; // Increase platforms speed
        }
    }

    public void SpawnPlatform()
    {
        GameObject platform = poolPlatforms.GetRandomPlatform();
        platform.SetActive(true);

        if (count == currentHighScore) // About to surpass current high score
        {
            foreach (MeshRenderer meshRenderer in platform.GetComponentsInChildren<MeshRenderer>())
            {
                meshRenderer.material = platformHighScoreMaterial;
            }
        }
        count++;
    }

    public void ReleasePlatform(GameObject platform)
    {
        poolPlatforms.ReleasePlatform(platform);
    }

    public void Restart()
    {
        count = 0;
        speed = initialSpeed;
        timeElapsed = 0f;
        timeNext = 1f / spawnRateController.SpawnRate;

        currentHighScore = highScoreController.HighScore;

        poolPlatforms.ReleaseAllPlatforms();

        SpawnPlatform();
    }


    class PoolPlatformManager
    {
        Vector3 initialPosition;
        Material platformDefaultMaterial;

        private System.Random random;

        private List<GameObject> poolPlatformList;
        private List<int> platformsAvailable;
        private Dictionary<GameObject, int> platformsIndexes;
              
        public PoolPlatformManager(Vector3 initialPosition, Material platformDefaultMaterial, Transform parentTransform)
        {
            this.initialPosition = initialPosition;
            this.platformDefaultMaterial = platformDefaultMaterial;

            random = new System.Random(); //Chooses a random seed by default. This makes sure every playthrough is almost unique

            List<GameObject> prefabsPoolPlatformList = new List<GameObject>(Resources.LoadAll<GameObject>("Platforms"));

            poolPlatformList = new List<GameObject>(prefabsPoolPlatformList.Count);
            platformsAvailable = new List<int>(prefabsPoolPlatformList.Count);
            platformsIndexes = new Dictionary<GameObject, int>(prefabsPoolPlatformList.Count);

            for (int i = 0; i < prefabsPoolPlatformList.Count; ++i)
            {
                GameObject platform = Instantiate(prefabsPoolPlatformList[i]);
                platform.transform.parent = parentTransform;

                ResetPlatform(platform);

                poolPlatformList.Add(platform);
                platformsIndexes.Add(platform, i);
                platformsAvailable.Add(i);
            }
        }

        public GameObject GetRandomPlatform()
        {
            if (!(platformsAvailable.Count > 0))
            {
                return null;
            }

            int index = random.Next(0, platformsAvailable.Count);

            int indexPlatform = platformsAvailable[index];
            platformsAvailable[index] = platformsAvailable[platformsAvailable.Count - 1]; // Swap with last index element
            platformsAvailable.RemoveAt(platformsAvailable.Count - 1); // Remove then las index element (this operation is O(1))

            return poolPlatformList[indexPlatform];
        }

        public void ReleasePlatform(GameObject platform)
        {
            ResetPlatform(platform);

            int index;
            platformsIndexes.TryGetValue(platform, out index);

            platformsAvailable.Add(index);
        }

        public void ReleaseAllPlatforms()
        {
            platformsAvailable.Clear();

            for (int i = 0; i < poolPlatformList.Count; ++i)
            {
                ResetPlatform(poolPlatformList[i]);
                platformsAvailable.Add(i);
            }
        }

        void ResetPlatform(GameObject platform)
        {
            platform.SetActive(false);
            platform.transform.SetPositionAndRotation(initialPosition, Quaternion.identity);
            foreach (MeshRenderer meshRenderer in platform.GetComponentsInChildren<MeshRenderer>())
            {
                meshRenderer.material = platformDefaultMaterial;
            }
        }
    }
}
