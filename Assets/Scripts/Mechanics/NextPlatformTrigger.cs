using UnityEngine;

/// <summary>
/// Spawns the next platform whenever a platform hits this gameObjects trigger collider.
/// This implementation makes keeping a constant distance between platforms 
/// </summary>
public class NextPlatformTrigger : MonoBehaviour
{
    ActivePlatformsController activePlatformsController;

    private void Awake()
    {
        activePlatformsController = FindObjectOfType<ActivePlatformsController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("MovingPlatform"))
        {
            activePlatformsController.SpawnPlatform();
        }
    }
}
