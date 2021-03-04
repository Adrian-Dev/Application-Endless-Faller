using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles what to do when either when the player or any platform reach the boundary of the level
/// This is being used to implement what happens when these elements are out of the screen.
/// </summary>
public class BoundaryController : MonoBehaviour
{
    LevelController _levelController;
    MainCharacterController _mainCharacterController;

    public void InjectDependencies(LevelController levelController, MainCharacterController mainCharacterController)
    {
        _levelController = levelController;
        _mainCharacterController = mainCharacterController;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            StartCoroutine(LoseGame());
        }
    }

    IEnumerator LoseGame()
    {
        _levelController.SetGameLost();
        _mainCharacterController.Explode();
        yield return new WaitForSeconds(0.5f);
        _levelController.AllowPauseGame(false);
        _levelController.PauseGame(true);
        yield return null;
    }
}
