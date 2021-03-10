using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Handles the high score in the application
/// </summary>
public class HighScoreController : MonoBehaviour
{
    string fileName = "highScore.bin";

    int _highScore;
    public int HighScore
    {
        get { return _highScore; }
    }

    public void LoadCurrentHighScore()
    {
        HighScoreData highScoreData = SavingSystem<HighScoreData>.Load(fileName);
        _highScore = highScoreData.HighScore;
    }

    public void SetNewHighScore(int newHighScore)
    {
        _highScore = newHighScore;
        SavingSystem<HighScoreData>.Save(new HighScoreData(_highScore), fileName);
    }

    public void WriteHighScoreOnText(Text highScoreText)
    {
        highScoreText.text = _highScore.ToString();
    }

}
