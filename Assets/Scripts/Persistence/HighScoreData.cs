using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class HighScoreData
{
    public int highScore;

    public HighScoreData(HighScoreController highScoreController)
    {
        highScore = highScoreController.highScore;
    }
}
