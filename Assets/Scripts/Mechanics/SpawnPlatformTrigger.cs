using UnityEngine;

/// <summary>
/// Spawns the next platform whenever a platform hits this gameObjects trigger collider.
/// This implementation makes platforms keep a constant distance between them 
/// </summary>
public class SpawnPlatformTrigger : MonoBehaviour
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
