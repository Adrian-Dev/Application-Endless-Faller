using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseController : MonoBehaviour
{
    [SerializeField] GameObject world;
    [SerializeField] GameObject menu;
    [SerializeField] GameObject UI;

    private List<MonoBehaviour> worldMonoBehaviours;
    private List<AudioSource> audioSources;

    private void Awake()
    {
        worldMonoBehaviours = new List<MonoBehaviour>();
        
        var WorldMonoBehaviours = world.GetComponentsInChildren<MonoBehaviour>();
        foreach (var monoBehaviour in WorldMonoBehaviours)
        {
            //Ignore PostProcessLayers so they won't be deactivated
            if (!monoBehaviour.GetType().FullName.Equals("UnityEngine.Rendering.PostProcessing.PostProcessLayer"))
            {
                worldMonoBehaviours.Add(monoBehaviour);
            }
        }

        audioSources = new List<AudioSource>(world.GetComponentsInChildren<AudioSource>());
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        SetActiveScripts(false);

        UI.SetActive(false);
        menu.SetActive(true);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        SetActiveScripts(true);

        UI.SetActive(true);
        menu.SetActive(false);
    }

    void SetActiveScripts(bool active)
    {
        foreach (var monoBehaviour in worldMonoBehaviours)
        {
            monoBehaviour.enabled = active;
        }

        foreach (var audioSource in audioSources)
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
