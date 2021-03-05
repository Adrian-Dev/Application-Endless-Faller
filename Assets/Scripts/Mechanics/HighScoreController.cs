using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Handles the high score in the application
/// </summary>
public class HighScoreController : MonoBehaviour
{
    int _highScore;
    public int HighScore
    {
        get { return _highScore; }
    }

    void Awake()
    {
        LoadCurrentHighScore();
    }

    public void LoadCurrentHighScore()
    {
        HighScoreData highScoreData = SaveSystem.LoadHighScore(this);
        _highScore = highScoreData.highScore;
    }

    public void SetNewHighScore(int newHighScore)
    {
        _highScore = newHighScore;
        SaveSystem.SaveHighScore(this);
    }

    public void WriteHighScoreOnText(Text highScoreText)
    {
        highScoreText.text = _highScore.ToString();
    }
}
