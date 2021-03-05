using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles the pause and resume events in the application
/// </summary>
public class PauseController 
{
    GameObject _world;
    GameObject _menu;
    GameObject _UI;

    private List<MonoBehaviour> _worldMonoBehaviours;
    private List<AudioSource> _audioSources;

    public void InjectDependencies(GameObject world, GameObject menu, GameObject UI)
    {
        _world = world;
        _menu = menu;
        _UI = UI;

        _worldMonoBehaviours = new List<MonoBehaviour>();

        var WorldMonoBehaviours = _world.GetComponentsInChildren<MonoBehaviour>();
        foreach (var monoBehaviour in WorldMonoBehaviours)
        {
            //Ignore PostProcessLayers so they won't be deactivated
            if (!monoBehaviour.GetType().FullName.Equals("UnityEngine.Rendering.PostProcessing.PostProcessLayer"))
            {
                _worldMonoBehaviours.Add(monoBehaviour);
            }
        }

        _audioSources = new List<AudioSource>(_world.GetComponentsInChildren<AudioSource>());
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        SetActiveScripts(false);

        _UI.SetActive(false);
        _menu.SetActive(true);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        SetActiveScripts(true);

        _UI.SetActive(true);
        _menu.SetActive(false);
    }

    void SetActiveScripts(bool active)
    {
        foreach (var monoBehaviour in _worldMonoBehaviours)
        {
            monoBehaviour.enabled = active;
        }

        foreach (var audioSource in _audioSources)
        {
            if (active)
            {
                audioSource.UnPause();
            }
            else
            {
                audioSource.Pause();
            }
        }
    }
    
}
