using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles what to do when either when the player or any platform reach the boundary of the level
/// This is being used to implement what happens when these elements are out of the screen.
/// </summary>
public class BoundaryTrigger : MonoBehaviour
{
    public bool Triggered;
    public string TagCollided;

    void Start()
    {
        ResetTrigger();
    }

    public void SetTag(string tag)
    {
        TagCollided = tag;
    }

    public void ResetTrigger()
    {
        Triggered = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals(TagCollided))
        {
            Triggered = true;
        }
    }
}
