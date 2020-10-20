using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

namespace BioTower.Structures
{

[SelectionBase]
public class DNABase : Structure
{   
    private void LevelLoaded()
    {
        GameManager.Instance.RegisterPlayerBase(this);
    }

    private void OnEnable()
    {
        GameManager.onLevelLoaded_01 += LevelLoaded;
    }

    private void OnDisable()
    {
        GameManager.onLevelLoaded_01 += LevelLoaded;
    }
}
}

