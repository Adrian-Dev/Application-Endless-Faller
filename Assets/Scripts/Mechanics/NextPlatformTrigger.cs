using UnityEngine;

/// <summary>
/// Spawns the next platform whenever a platform hits this gameObjects trigger collider.
/// This implementation makes platforms keep a constant distance between them 
/// </summary>
public class NextPlatformTrigger : MonoBehaviour
{
    PlatformsController _platformsController;

    public void InjectDependencies(PlatformsController platformsController)
    {
        _platformsController = platformsController;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("MovingPlatform"))
        {
            _platformsController.SpawnPlatform();
        }
    }
}
