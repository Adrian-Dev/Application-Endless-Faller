using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

/// <summary>
/// Loading and saving utility for generic serializable objects
/// </summary>
/// <typeparam name="T"></typeparam>
public static class SavingSystem<T> where T : class, new()
{
    private static string _path = Application.persistentDataPath;

    public static void Save(T instace, string fileName)
    {
        string tempFilePath = _path + "/" + fileName;

        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(tempFilePath, FileMode.Create);

        formatter.Serialize(stream, instace);
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

        T instace = formatter.Deserialize(stream) as T;
        stream.Close();

        return instace;
    }
}
