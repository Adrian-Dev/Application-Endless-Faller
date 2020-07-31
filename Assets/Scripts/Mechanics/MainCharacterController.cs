using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharacterController : MonoBehaviour
{
    [SerializeField] private float speed;

    Vector3 initialTransform;

    private void Awake()
    {
        initialTransform = transform.position;
    }

    void FixedUpdate()
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
