using UnityEngine;

/// <summary>
/// Platform logic
/// </summary>
public class MovingPlatform : MonoBehaviour
{
    LevelController levelController;
    PlatformSpawnController platformSpawnController;

    private void Awake()
    {
        levelController = FindObjectOfType<LevelController>();
        platformSpawnController = FindObjectOfType<PlatformSpawnController>();
    }

    void Update()
    {
        transform.Translate(Vector3.up * platformSpawnController.Speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            levelController.IncrementScore();
        }
    }
}

