using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Wraps the behaviour ...
/// </summary>
public class CollidedWithTarget : MonoBehaviour
{
    public bool Collided { get { return _collided; } }
    bool _collided;
    
    public string TargetTag { get { return _targetTag; } }
    string _targetTag;


    void Start()
    {
        ResetCollided();
    }

    public void ResetCollided()
    {
        _collided = false;
    }

    public void SetTargetTag(string targetTag)
    {
        _targetTag = targetTag;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals(_targetTag))
        {
            _collided = true;
        }
    }
}
