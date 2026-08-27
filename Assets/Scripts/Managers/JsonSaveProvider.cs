using System;
using System.IO;
using UnityEngine;

public class JsonSaveProvider : ISaveProvider
{
    readonly string _path = Path.Combine(Application.persistentDataPath, "save.json");

    public void Save(SaveData data)
    {
        data.lastSaveTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        File.WriteAllText(_path, JsonUtility.ToJson(data, prettyPrint: true));
    }

    public SaveData Load()
    {
        if (!File.Exists(_path)) return new SaveData();
        return JsonUtility.FromJson<SaveData>(File.ReadAllText(_path)) ?? new SaveData();
    }
}
