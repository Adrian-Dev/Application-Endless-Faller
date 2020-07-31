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
        ReadHighScore();
    }

    public void ReadHighScore()
    {        
        //TODO read actual value 
        highScore = 2;
        highScoreText.text = highScore.ToString();
    }

    public void SetNewHighScore(int newHighScore)
    {
        //Persist value to disk
    }

}
