using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundaryController : MonoBehaviour
{
    LevelController levelController;
    MainCharacterController mainCharacterController;
    private void Awake()
    {
        levelController = FindObjectOfType<LevelController>();
        mainCharacterController = FindObjectOfType<MainCharacterController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            StartCoroutine(LoseGame());
        }
        else if (other.tag.Equals("MovingPlatform"))
        {
            Destroy(other.transform.parent.gameObject);
        }
    }

    IEnumerator LoseGame()
    {
        levelController.SetGameLost();
        mainCharacterController.Explosion();
        yield return new WaitForSeconds(0.5f);
        levelController.AllowPauseGame(false);
        levelController.PauseGame(true);
        yield return null;
    }
}
