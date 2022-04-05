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

        public void LoadAndSave()
        {
            var gameData = Load();
            gameData.settings = Util.upgradeSettings;
            Save(gameData);
        }

        [Button("Load data")]
        public GameData Load()
        {
            GameData gameData = null;
            if (File.Exists(dataPath))
            {
                string fileContents = File.ReadAllText(dataPath);
                gameData = JsonUtility.FromJson<GameData>(fileContents);
                Debug.Log($"<color=cyan>Load Data. Data loaded successfully</color>");
                return gameData;
            }
            else
            {
                gameData = new GameData();
                gameData.settings = Util.gameSettings.defaultSettings;
                gameData.settings.currLevel = 1;
                string jsonString = JsonUtility.ToJson(gameData, true);

                if (!Directory.Exists(dataDirectory))
                {
                    Directory.CreateDirectory(dataDirectory);
                }

                //gameData = new GameData();
                //gameData.settings.currLevel = 1;
                jsonString = JsonUtility.ToJson(gameData, true);
                File.WriteAllText(dataPath, jsonString);
                Debug.Log($"<color=cyan>Load Data. File doesn't exist. Create a new one with default data and read write it to the file</color>");

#if UNITY_EDITOR
                UnityEditor.AssetDatabase.Refresh();
#endif

                return gameData;
            }
        }

        [Button("Reset Save")]
        public void ResetSave()
        {
            var gameData = new GameData();
            gameData.settings = Util.gameSettings.defaultSettings;
            gameData.settings.currLevel = 1;

            if (Application.isPlaying)
            {
                //Util.gameSettings.defaultSettings = gameData.settings;
                Util.gameSettings.upgradeSettings = Util.gameSettings.defaultSettings;
            }

            //gameData.currLevel = 0;
            Save(gameData);
        }

        private void OnSpendCurrency(int numSpent, int currentCurrency)
        {
            Util.upgradeSettings.energy = currentCurrency;
        }

        private void OnGainCurrency(int numGained, int currentCurrency)
        {
            Util.upgradeSettings.energy = currentCurrency;
        }

        private void OnEnable()
        {
            EventManager.Game.onSpendCurrency += OnSpendCurrency;
            EventManager.Game.onGainCurrency += OnGainCurrency;
        }

        private void OnDisable()
        {
            EventManager.Game.onSpendCurrency -= OnSpendCurrency;
            EventManager.Game.onGainCurrency -= OnGainCurrency;
        }
    }
}