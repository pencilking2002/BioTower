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
            SceneManager.LoadScene(0);
        }
        else
        {
            BootController.isBootLoaded = false;
        }
    }
}
}