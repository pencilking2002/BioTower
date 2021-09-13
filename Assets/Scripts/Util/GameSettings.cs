using UnityEngine;

namespace BioTower
{
[CreateAssetMenu(fileName="GameSettings", menuName="GameSettings")]
public class GameSettings : ScriptableObject
{
    public Params defaultSettings;
    public Params upgradeSettings;
}
}
