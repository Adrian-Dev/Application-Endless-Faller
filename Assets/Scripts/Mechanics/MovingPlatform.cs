using UnityEngine;

/// <summary>
/// Platform logic
/// </summary>
public class MovingPlatform : MonoBehaviour
{
    public bool CollidedWithPlayer;
    public bool CollidedWitBoundary;

    private void Start()
    {
        ResetTrigger();
    }

    public void ResetTrigger()
    {
        CollidedWithPlayer = false;
        CollidedWitBoundary = false;
    }

    public void MoveUp(float speed)
    {
        transform.Translate(Vector3.up * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            CollidedWithPlayer = true;
        }
        else if (other.tag.Equals("Boundary"))
        {
            CollidedWitBoundary = true;
        }
    }

    public void ChangeMaterial(Material material)
    {
        foreach (MeshRenderer meshRenderer in GetComponentsInChildren<MeshRenderer>())
        {
            meshRenderer.material = material;
        }
    }
}

