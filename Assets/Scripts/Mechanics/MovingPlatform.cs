using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Platform logic
/// </summary>
public class MovingPlatform : MonoBehaviour
{
    public CollidedWithTarget CollidedWithPlayer { get { return _collidedWithPlayer; } }
    [SerializeField] CollidedWithTarget _collidedWithPlayer;
    public CollidedWithTarget CollidedWithBoundary { get { return _collidedWithBoundary; } }
    [SerializeField] CollidedWithTarget _collidedWithBoundary;

    List<Renderer> _renderers;

    private void Awake()
    {
        _renderers = new List<Renderer>(GetComponentsInChildren<MeshRenderer>());
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

