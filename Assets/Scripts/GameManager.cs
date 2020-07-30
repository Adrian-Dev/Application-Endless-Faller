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
        StartCoroutine(LoadScene(gameScene));
    }

    private IEnumerator LoadScene(string sceneName)
    {
        Debug.Log("Loading scene named: '" + sceneName + "'");
        yield return new WaitForSeconds(0.1f);
        EditorSceneManager.LoadScene(sceneName);
    }
}