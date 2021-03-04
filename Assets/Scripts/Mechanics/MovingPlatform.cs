using UnityEngine;

/// <summary>
/// Platform logic
/// </summary>
public class MovingPlatform : MonoBehaviour
{
    LevelController _levelController;
    PlatformsController _platformsController;

    public void InjectDependencies(LevelController levelController, PlatformsController platformsController)
    {
        _levelController = levelController;
        _platformsController = platformsController;
    }

    void Update()
    {
        MoveUp();
    }

    void MoveUp()
    {
        transform.Translate(Vector3.up * _platformsController.Speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            _levelController.IncrementScore();
        }
        else if (other.tag.Equals("Boundary"))
        {
            _platformsController.ReleasePlatform(this); // Probably not the best place to do this, as the object should not be concerned about creating and releasing/destroying itself (its life cycle)
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

