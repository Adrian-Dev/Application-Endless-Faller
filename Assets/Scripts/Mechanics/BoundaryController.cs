using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles what to do when either when the player or any platform reach the boundary of the level
/// This is being used to implement what happens when these elements are out of the screen.
/// </summary>
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
            levelController.SetGameLost();
            mainCharacterController.Explosion();
            StartCoroutine(LoseGame());
        }
    }

    IEnumerator LoseGame()
    {
        yield return new WaitForSeconds(0.5f);
        levelController.AllowPauseGame(false);
        levelController.PauseGame(true);
        yield return null;
    }
}
