using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
