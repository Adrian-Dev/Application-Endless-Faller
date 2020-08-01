using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Spawns the next platform whenever a platform hits this gameObjects trigger collider.
/// This implementation makes keeping a constant distance between platforms 
/// </summary>
public class NextPlatformController : MonoBehaviour
{
    PlatformSpawnController platformSpawnController;

    private void Awake()
    {
        platformSpawnController = FindObjectOfType<PlatformSpawnController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("MovingPlatform"))
        {
            platformSpawnController.SpawnPlatform();
        }
    }
}
