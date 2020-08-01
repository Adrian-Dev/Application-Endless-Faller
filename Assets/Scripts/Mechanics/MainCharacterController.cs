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

    private void Awake()
    {
        initialTransform = transform.position;
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
        GetComponent<ParticleSystem>().Play();
    }

    public void Restart()
    {
        transform.position = initialTransform;
        GetComponent<Rigidbody>().velocity = new Vector3(0f, 0f, 0f); // Restore velocity, otherwise will continue with current falling speed by gravity
    }
}
