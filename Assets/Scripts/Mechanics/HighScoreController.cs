using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Handles the high score in the application
/// </summary>
public class HighScoreController : MonoBehaviour
{
    [Tooltip("Refence to high score in the UI")]
    [SerializeField] Text highScoreText;

    int highScore;

    public int HighScore
    {
        get { return highScore; }
    }

    private void Awake()
    {
        highScore = 0;
        ReadHighScore();
    }

    public void ReadHighScore()
    {
        HighScoreData highScoreData = SaveSystem.LoadHighScore(this);

        highScore = highScoreData.highScore;
        highScoreText.text = highScore.ToString();
    }

    public void SetNewHighScore(int newHighScore)
    {
        highScore = newHighScore;
        SaveSystem.SaveHighScore(this);
    }

}
