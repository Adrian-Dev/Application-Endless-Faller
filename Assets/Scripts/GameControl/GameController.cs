using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEditor.SceneManagement;

public class GameController : MonoBehaviour
{
    [SerializeField] private string gameScene;

    public void Play()
    {
        StartCoroutine(LoadScene(gameScene));
    }

    private IEnumerator LoadScene(string sceneName)
    {
        Debug.Log("Loading game!");
        yield return new WaitForSeconds(14);
        EditorSceneManager.LoadScene(sceneName);
    }
}
