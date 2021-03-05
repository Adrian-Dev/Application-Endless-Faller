using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Player Controller
/// </summary>
public class MainCharacterController : MonoBehaviour
{
    [Tooltip("Movement speed")]
    [SerializeField] float _speed;

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

    public void MoveLeft()
    {
        transform.Translate(Vector3.left * _speed * Time.deltaTime);
    }

    public void MoveRight()
    {
        transform.Translate(Vector3.right * _speed * Time.deltaTime);
    }

    public void MoveUp()
    {
        transform.Translate(Vector3.up * _speed * Time.deltaTime);
    }

    public void MoveDown()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
    }

    public void Explode()
    {
        _explosionParticleSystem.Play();
    }
}
