using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

/// <summary>
/// Loading and saving utility
/// </summary>
public static class SaveSystem
{
    private static string _path = Application.persistentDataPath + "/highScore.bin";

    public static void SaveHighScore(HighScoreController highScoreController)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(_path, FileMode.Create);

        HighScoreData highScoreData = new HighScoreData(highScoreController);

        formatter.Serialize(stream, highScoreData);
        stream.Close();
    }

    public static HighScoreData LoadHighScore(HighScoreController highScoreController)
    {
        if (!File.Exists(_path))
        {
            Debug.Log("File " + _path + " not found. Creating it");

            SaveHighScore(highScoreController);
        }

        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(_path, FileMode.Open);

        HighScoreData highScoreData = formatter.Deserialize(stream) as HighScoreData;
        stream.Close();

        return highScoreData;
    }
}
