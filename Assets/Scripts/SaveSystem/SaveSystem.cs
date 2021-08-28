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
    public void Save(GameData inputGameData)
    {
        #if UNITY_EDITOR
        if (!Application.isPlaying)
            dataPath = "C:/Repos/BioTower/Assets/Data/gamedata.json";
        #endif

        if (File.Exists(dataPath))
        {
            string jsonString = JsonUtility.ToJson(inputGameData, true);
            File.WriteAllText(dataPath, jsonString);
            
            #if UNITY_EDITOR            
            UnityEditor.AssetDatabase.Refresh();
            #endif

            Debug.Log($"<color=cyan>Save data. Overwrite file</color>");
        }
        else
        {
            var dataDir = dataDirectory;
            if (!Application.isPlaying)
                dataDir = "C:/Repos/BioTower/Assets/Data/";
        
            if (!Directory.Exists(dataDir))
            {
                Directory.CreateDirectory(dataDir);
            }

            string jsonString = JsonUtility.ToJson(inputGameData, true);
            File.WriteAllText(dataPath, jsonString);

            #if UNITY_EDITOR
            UnityEditor.AssetDatabase.Refresh();
            #endif

            Debug.Log($"<color=cyan>Save data. File doesn't exist. Create new file and fill it with default data</color>");
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
            gameData.currLevel = 0;

            string jsonString = JsonUtility.ToJson(gameData, true);
            File.WriteAllText(dataPath, jsonString);
            Debug.Log($"<color=cyan>Load Data. File doesn't exist. Create a new one with default data and read write it to the file</color>");

            #if UNITY_EDITOR
            UnityEditor.AssetDatabase.Refresh();
            #endif

            return gameData;
        }
    }

    [Button("Reset Save")]
    public void ResetQuests()
    {
        var gameData = new GameData();
        //gameData.currLevel = 0;
        Save(gameData); 
    }

    // [Button("Delete Data Directory")]
    // public void DeleteDataDirectory()
    // {
    //     Directory.Delete(dataDirectory);
    // }

    // private GameData ProcessGameData(GameData gameData)
    // {
    //     return gameData;
    // }
}
}