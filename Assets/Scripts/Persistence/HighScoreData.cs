using System.Collections;
using System.Collections.Generic;

[System.Serializable]

/// <summary>
/// High Score Data encapsulation
/// </summary>
public class HighScoreData
{
    public int highScore;

    public HighScoreData(HighScoreController highScoreController)
    {
        highScore = highScoreController.HighScore;
    }
}
