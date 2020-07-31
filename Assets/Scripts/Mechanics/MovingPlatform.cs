using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    LevelController levelController;
    PlatformSpawnController platformSpawnController;

    private void Awake()
    {
        levelController = FindObjectOfType<LevelController>();
        platformSpawnController = FindObjectOfType<PlatformSpawnController>();
    }

    void FixedUpdate()
    {
        transform.Translate(Vector3.up * platformSpawnController.speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            levelController.IncrementScore();
        }
    }
}

