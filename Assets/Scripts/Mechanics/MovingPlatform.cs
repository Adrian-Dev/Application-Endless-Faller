using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Platform logic
/// </summary>
public class MovingPlatform : MonoBehaviour
{
    public List<CollidedWithTarget> CollidedWithTargets { get { return _collidedWithTargets; } }
    List<CollidedWithTarget> _collidedWithTargets;

    List<Renderer> _renderers;

    void Awake()
    {
        _renderers = new List<Renderer>(GetComponentsInChildren<MeshRenderer>());
        _collidedWithTargets = new List<CollidedWithTarget>(GetComponents<CollidedWithTarget>());
    }

    public void MoveUp(float speed)
    {
        transform.Translate(Vector3.up * speed * Time.deltaTime);
    }

    public void ChangeMaterial(Material material)
    {
        foreach (Renderer renderer in _renderers)
        {
            renderer.material = material;
        }
    }
}

