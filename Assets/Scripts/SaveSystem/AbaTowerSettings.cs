using System;

namespace BioTower.SaveData
{
[Serializable]
public class AbaTowerSettings
{
    public int abaMaxInfluenceRadius;
    public int abaMapScale;
    public int abaInfluenceShapeRadius;
    
    public int abaUnitCost;
    
    public int abaUnitSpawnLimit;


    public void SetAbaTowerInfluence(float influenceRadius, float mapScale, float shapeRadius)
    {
        this.abaMaxInfluenceRadius = (int) (influenceRadius * 1000);
        this.abaMapScale = (int) (mapScale * 1000);
        this.abaInfluenceShapeRadius = (int) (shapeRadius * 1000);
    }

    public void SetAbaUnitCost(int unitCost)
    {
        this.abaUnitCost = unitCost;
    }

    public void SetAbaUnitSpawnLimit(int spawnLimit)
    {
        this.abaUnitSpawnLimit = spawnLimit;
    }
}
}