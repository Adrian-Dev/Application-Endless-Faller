using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles the player behaviour
/// </summary>
public class MainCharacter : MonoBehaviour
{
    Vector3 _initialTransform;
    ParticleSystem _explosionParticleSystem;
    Rigidbody _rbody;

    void Awake()
    {
        _initialTransform = transform.position;
        _explosionParticleSystem = GetComponent<ParticleSystem>();
        _rbody = GetComponent<Rigidbody>();
    }
    public void Initialize()
    {
        transform.position = _initialTransform;
        _rbody.velocity = new Vector3(0f, 0f, 0f); // Restore velocity, otherwise will continue with current falling speed by gravity
    }

    public void Move(Vector3 moveVector)
    {
        transform.Translate(moveVector);
    }

    public void Explode()
    {
        _explosionParticleSystem.Play();
    }
}
