using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private float speed;

    LevelManager levelManager;

    private void Awake()
    {
        levelManager = FindObjectOfType<LevelManager>();
    }

    void FixedUpdate()
    {
        transform.Translate(Vector3.up * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            levelManager.IncrementScore();
        }
    }
}