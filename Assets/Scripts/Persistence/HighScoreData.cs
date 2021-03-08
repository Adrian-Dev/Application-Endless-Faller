using System.Collections;
using System.Collections.Generic;

[System.Serializable]

/// <summary>
/// High Score Data encapsulation to be saved
/// </summary>
public class HighScoreData
{
    public int HighScore;

    public HighScoreData()
    {
        HighScore = 0;
    }

    public HighScoreData(int highScore)
    {
        HighScore = highScore;
    }
}
