using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private float speed;

    void FixedUpdate()
    {
        transform.Translate(Vector3.up * speed * Time.deltaTime);
    }
}