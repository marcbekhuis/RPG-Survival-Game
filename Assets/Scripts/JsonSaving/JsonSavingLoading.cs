using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public static class JsonSavingLoading
{
    public static void SaveData<T>(T data, string folder, string file)
    {
        string dataString = JsonUtility.ToJson(data);
        if (!Directory.Exists(Application.persistentDataPath + "/" + folder))
            Directory.CreateDirectory(Application.persistentDataPath + "/" + folder);
        File.WriteAllText(Application.persistentDataPath + "/" + folder + "/" + file + ".txt" ,dataString);
    }

    public static T LoadData<T>(string folder, string file)
    {
        if (Directory.Exists(Application.persistentDataPath + "/" + folder))
            if (File.Exists(Application.persistentDataPath + "/" + folder + "/" + file + ".txt"))
                return JsonUtility.FromJson<T>(File.ReadAllText(Application.persistentDataPath + "/" + folder + "/" + file + ".txt"));
        return default;
    }

    public static string LoadData(string folder, string file)
    {
        if (Directory.Exists(Application.persistentDataPath + "/" + folder))
            if (File.Exists(Application.persistentDataPath + "/" + folder + "/" + file + ".txt"))
                return File.ReadAllText(Application.persistentDataPath + "/" + folder + "/" + file + ".txt");
        return default;
    }
}
