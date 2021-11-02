using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BioTower
{
public class BootLoader : MonoBehaviour
{
    private void Awake()
    {
        if (!BootController.isBootLoaded)
        {
            SceneManager.LoadScene(0, LoadSceneMode.Additive);
            BootController.isBootLoaded = true;
        }
    }

    private void Start()
    {
        if (Util.bootController != null)
            Util.bootController.SetGameState();
    }
}
}