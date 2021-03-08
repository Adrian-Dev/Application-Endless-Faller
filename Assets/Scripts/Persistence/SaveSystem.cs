using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

/// <summary>
/// Loading and saving utility
/// </summary>
public static class SaveSystem
{
    private static string _path = Application.persistentDataPath + "/highScore.bin";

    public static void SaveHighScore(HighScoreData highScoreData)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(_path, FileMode.Create);

        formatter.Serialize(stream, highScoreData);
        stream.Close();
    }

    public static HighScoreData LoadHighScore()
    {
        if (!File.Exists(_path))
        {
            Debug.Log("File " + _path + " not found. Creating it");

            SaveHighScore(new HighScoreData()); // Saves new High Score in a new file with default value
        }

        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(_path, FileMode.Open);

        HighScoreData highScoreData = formatter.Deserialize(stream) as HighScoreData;
        stream.Close();

        return highScoreData;
    }
}
