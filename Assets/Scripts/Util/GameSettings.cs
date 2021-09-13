using UnityEngine;
using BioTower.SaveData;

namespace BioTower
{
[CreateAssetMenu(fileName="GameSettings", menuName="GameSettings")]
public class GameSettings : ScriptableObject
{
    public Params defaultSettings;
    public Params upgradeSettings;

    public void UpdateUpgradeSettings(GameData gameData)
    {
        upgradeSettings = gameData.settings;
        
        // ABA tower influence
        // abaUnitSpawnLimit = gameData.abaTowerSettings.abaUnitSpawnLimit;
        // abaMaxInfluenceRadius = gameData.abaTowerSettings.abaMaxInfluenceRadius / 1000;
        // abaMapScale = gameData.abaTowerSettings.abaMapScale / 1000;
        // abaInfluenceShapeRadius = gameData.abaTowerSettings.abaInfluenceShapeRadius / 1000;

        // more...
    }
}   
}
