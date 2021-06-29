using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Sirenix.OdinInspector;

namespace BioTower.SaveData
{
public class SaveSystem : MonoBehaviour
{
    public static string fileName = "gamedata.json";
    public static string dataDirectory;
    public static string dataPath;

    private void Awake()
    {
        #if UNITY_EDITOR

        dataDirectory = Application.dataPath + "/Data/";
        dataPath = dataDirectory + fileName;
        Debug.Log($"<color=cyan>Start save system. dataPath: {dataPath}</color>");

        #else

        dataDirectory = Application.persistentDataPath + "/Data/";
        dataPath = dataDirectory + fileName;
        Debug.Log($"<color=cyan>Start save system. dataPath: {dataPath}</color>");

        #endif
    }

    [Button("Save data")]
    public void Save()
    {
        if (File.Exists(dataPath))
        {
            // Get current data
            string fileContents = File.ReadAllText(dataPath);
            GameData gameData = JsonUtility.FromJson<GameData>(fileContents);

            // Process the data
            gameData = ProcessGameData(gameData);

            // Write data back into the file
            string jsonString = JsonUtility.ToJson(gameData);
            File.WriteAllText(dataPath, jsonString);
            Debug.Log($"<color=cyan>Save Data. File exists. Write {jsonString} into save file</color>");

            #if UNITY_EDITOR
            UnityEditor.AssetDatabase.Refresh();
            #endif
        }
        else
        {
            if (!Directory.Exists(dataDirectory))
            {
                Directory.CreateDirectory(dataDirectory);
            }

            // Create a new file with with default data
            GameData gameData = new GameData();
            string jsonString = JsonUtility.ToJson(gameData);
            File.WriteAllText(dataPath, jsonString);
            Debug.Log($"<color=cyan>Save data. File doesn't exist. Create new file and fill it with default data</color>");

            #if UNITY_EDITOR
            UnityEditor.AssetDatabase.Refresh();
            #endif
        }
    }

    [Button("Load data")]
    public GameData Load()
    {
        if (File.Exists(dataPath))
        {
            string fileContents = File.ReadAllText(dataPath);
            GameData gameData = JsonUtility.FromJson<GameData>(fileContents);
            Debug.Log($"<color=cyan>Load Data. Data loaded successfully</color>");
            return gameData;
        }
        else
        {
            if (!Directory.Exists(dataDirectory))
            {
                Directory.CreateDirectory(dataDirectory);
            }

            GameData gameData = new GameData();
            string jsonString = JsonUtility.ToJson(gameData);
            File.WriteAllText(dataPath, jsonString);
            Debug.Log($"<color=cyan>Load Data. File doesn't exist. Create a new one with default data and read write it to the file</color>");

            return gameData;
        }
    }

    [Button("Delete Data Directory")]
    public void DeleteDataDirectory()
    {
        Directory.Delete(dataDirectory);
    }

    private GameData ProcessGameData(GameData gameData)
    {
        return gameData;
    }
}
}