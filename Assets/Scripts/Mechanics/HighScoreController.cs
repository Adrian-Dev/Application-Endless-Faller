using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighScoreController : MonoBehaviour
{
    [SerializeField] Text highScoreText;
    [SerializeField] public int highScore { get; private set; }

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
