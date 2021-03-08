using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

/// <summary>
/// Loading and saving utility
/// </summary>
/// <typeparam name="T"></typeparam>
public static class SaveSystem<T> where T : class, new()
{
    private static string _path = Application.persistentDataPath;

    public static void Save(T type, string fileName)
    {
        string tempFilePath = _path + "/" + fileName;

        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(tempFilePath, FileMode.Create);

        formatter.Serialize(stream, type);
        stream.Close();
    }

    public static T Load(string fileName)
    {
        string tempFilePath = _path + "/" + fileName;

        if (!File.Exists(tempFilePath))
        {
            Debug.Log("File " + tempFilePath + " not found. Creating it");

            Save(new T(), fileName);
        }

        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(tempFilePath, FileMode.Open);

        T type = formatter.Deserialize(stream) as T;
        stream.Close();

        return type;
    }
}
