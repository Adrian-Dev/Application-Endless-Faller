using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Player Controller
/// </summary>
public class MainCharacterController : MonoBehaviour
{
    [Tooltip("Movement speed")]
    [SerializeField] private float speed;

    Vector3 initialTransform;

    ParticleSystem explosionParticleSystem;
    Rigidbody rbody;

    private void Awake()
    {
        initialTransform = transform.position;
        explosionParticleSystem = GetComponent<ParticleSystem>();
        rbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        ProcessInput();
    }

    void ProcessInput()
    {
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            transform.Translate(Vector3.left * speed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            transform.Translate(Vector3.right * speed * Time.deltaTime);
        }
    }

    public void Explosion()
    {
        explosionParticleSystem.Play();
    }

    public void Restart()
    {
        transform.position = initialTransform;
        rbody.velocity = new Vector3(0f, 0f, 0f); // Restore velocity, otherwise will continue with current falling speed by gravity
    }
}
