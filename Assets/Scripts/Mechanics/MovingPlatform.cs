using UnityEngine;

/// <summary>
/// Platform logic
/// </summary>
public class MovingPlatform : MonoBehaviour
{
    LevelController levelController;
    ActivePlatformsController activePlatformsController;

    private void Awake()
    {
        levelController = FindObjectOfType<LevelController>();
        activePlatformsController = FindObjectOfType<ActivePlatformsController>();
    }

    void Update()
    {
        transform.Translate(Vector3.up * activePlatformsController.Speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            levelController.IncrementScore();
        }
        else if (other.tag.Equals("Boundary"))
        {
            activePlatformsController.ReleasePlatform(gameObject);
        }
    }
}

