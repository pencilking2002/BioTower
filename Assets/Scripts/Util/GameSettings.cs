using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="GameSettings", menuName="GameSettings")]
public class GameSettings : ScriptableObject
{
    [Range(1,10)]public float timeScale = 1.0f;
}
