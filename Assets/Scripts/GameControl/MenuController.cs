using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Class served as an interface for the Menu and the application game flow controller
/// </summary>
public class MenuController : MonoBehaviour
{
    LevelController levelController;

    private void Awake()
    {
        levelController = FindObjectOfType<LevelController>();
    }

    public void Resume()
    {
        levelController.ResumeGame();
    }

    public void Restart()
    {
        levelController.Initialize();
    }

    public void GoToScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
