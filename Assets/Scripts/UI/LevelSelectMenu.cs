using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace BioTower
{
public class LevelSelectMenu : MonoBehaviour
{
    public Canvas canvas;
    [SerializeField] private SceneReference[] levelScenes;
    [SerializeField] private Button[] levelSelectButtons;
    
    public void LoadLevel(int levelIndex)
    {
        Debug.Log("Press Button");
        SceneManager.LoadScene(levelScenes[levelIndex].ScenePath, LoadSceneMode.Additive);
        EventManager.UI.onTapLevelSelectButton?.Invoke();
    }
}
}