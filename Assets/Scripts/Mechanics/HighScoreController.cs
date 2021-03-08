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

    public void LoadCurrentHighScore()
    {
        HighScoreData highScoreData = SaveSystem.LoadHighScore();
        _highScore = highScoreData.HighScore;
    }

    public void SetNewHighScore(int newHighScore)
    {
        _highScore = newHighScore;
        SaveSystem.SaveHighScore(new HighScoreData(_highScore));        
    }

    public void WriteHighScoreOnText(Text highScoreText)
    {
        highScoreText.text = _highScore.ToString();
    }

}
