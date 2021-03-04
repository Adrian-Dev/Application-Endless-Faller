using UnityEngine;

/// <summary>
/// Spawns the next platform whenever a platform hits this gameObjects trigger collider.
/// This implementation makes platforms keep a constant distance between them 
/// </summary>
public class NextPlatformTrigger : MonoBehaviour
{
    ActivePlatformsController _activePlatformsController;

    public void InjectDependencies(ActivePlatformsController activePlatformsController)
    {
        _activePlatformsController = activePlatformsController;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("MovingPlatform"))
        {
            _activePlatformsController.SpawnPlatform();
        }
    }
}
