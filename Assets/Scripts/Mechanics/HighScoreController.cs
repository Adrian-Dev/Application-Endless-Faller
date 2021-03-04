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

    int _highScore;

    public int HighScore
    {
        get { return _highScore; }
    }

    private void Awake()
    {
        _highScore = 0;
        ReadHighScore();
    }

    public void ReadHighScore()
    {
        HighScoreData highScoreData = SaveSystem.LoadHighScore(this);

        _highScore = highScoreData.highScore;
        highScoreText.text = _highScore.ToString();
    }

    public void SetNewHighScore(int newHighScore)
    {
        _highScore = newHighScore;
        SaveSystem.SaveHighScore(this);
    }

}
