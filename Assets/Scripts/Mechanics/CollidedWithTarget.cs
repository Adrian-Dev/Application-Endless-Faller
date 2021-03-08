using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class CollidedWithTarget : MonoBehaviour
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
