using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEditor.SceneManagement;

/// <summary> Manages the state of the whole application </summary>
public class GameManager : MonoBehaviour
{
    [SerializeField] private string gameScene;

    public void Play()
    {
        SceneManager.LoadScene(gameScene);

        //StartCoroutine(LoadScene(gameScene));
    }

    //TODO Check not working after trying to load when coming from the Game scene
    private IEnumerator LoadScene(string sceneName)
    {
        Debug.Log("Loading scene named: '" + sceneName + "'");
        yield return new WaitForSeconds(1.5f);
        //SceneManager.LoadScene(gameScene);
        EditorSceneManager.LoadScene(sceneName);
    }
}