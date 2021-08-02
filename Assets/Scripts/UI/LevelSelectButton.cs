using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

namespace BioTower.UI
{
public class LevelSelectButton : MonoBehaviour
{
    public SceneReference sceneToLoad;
    public bool isUnlocked;
    [HideInInspector] public Button button;
    [HideInInspector] public TextMeshProUGUI btnText;
    [HideInInspector] public Image lockIcon;

    private void Awake()
    {
        button = GetComponent<Button>();
        btnText = transform.Find("Text").GetComponent<TextMeshProUGUI>();
        lockIcon = transform.Find("Lock").GetComponent<Image>();
    }

    public void LoadLevel()
    {
        if (!isUnlocked)
            return;
            
        Debug.Log("Press Button");
        SceneManager.LoadScene(sceneToLoad.ScenePath, LoadSceneMode.Additive);
        EventManager.UI.onTapLevelSelectButton?.Invoke();
    }

    public void Lock()
    {
        btnText.gameObject.SetActive(false);
        lockIcon.gameObject.SetActive(true);
        isUnlocked = false;
    }

    public void Unlock()
    {
        btnText.gameObject.SetActive(true);
        lockIcon.gameObject.SetActive(false);
        isUnlocked = true;
    }
}
}
