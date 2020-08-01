using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

/// <summary>
/// Loading and saving utility
/// </summary>
public static class SaveSystem
{
    private static string path = Application.persistentDataPath + "/highScore.bin";

    public static void SaveHighScore(HighScoreController highScoreController)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(path, FileMode.Create);

        HighScoreData highScoreData = new HighScoreData(highScoreController);

        formatter.Serialize(stream, highScoreData);
        stream.Close();
    }

    public static HighScoreData LoadHighScore(HighScoreController highScoreController)
    {
        if (!File.Exists(path))
        {
            Debug.Log("File " + path + " not found. Creating it");

            SaveHighScore(highScoreController);
        }

        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(path, FileMode.Open);

        HighScoreData highScoreData = formatter.Deserialize(stream) as HighScoreData;
        stream.Close();

        return highScoreData;
    }
}
