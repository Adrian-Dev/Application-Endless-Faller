using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundaryController : MonoBehaviour
{
    LevelManager levelManager;
    private void Awake()
    {
        levelManager = FindObjectOfType<LevelManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            levelManager.Reset();
        }
        else if (other.tag.Equals("MovingPlatform"))
        {
            Destroy(other.transform.parent.gameObject);
        }
    }
}
