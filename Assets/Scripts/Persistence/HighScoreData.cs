using System.Collections;
using System.Collections.Generic;

[System.Serializable]

/// <summary>
/// High Score Data encapsulation
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
    
    public static bool operator ==(HighScoreData hsd1, HighScoreData hsd2)
    {
        if ((object)hsd1 == null)
        {
            return (object)hsd2 == null;
        }

        return hsd1.Equals(hsd2);
    }

    public static bool operator !=(HighScoreData hsd1, HighScoreData hsd2)
    {
        return !(hsd1 == hsd2);
    }

    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        HighScoreData hsd = (HighScoreData)obj;
        return (HighScore == hsd.HighScore);
    }

    public override int GetHashCode()
    {
        return HighScore.GetHashCode();
    }
    
}
