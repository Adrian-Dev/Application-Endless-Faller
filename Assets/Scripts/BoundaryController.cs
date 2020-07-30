using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundaryController : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            Destroy(other.gameObject);
        }
        //else if (other.tag.Equals("MovingPlatform"))
        //{
        //    Destroy(other.gameObject);
        //}
    }
}
