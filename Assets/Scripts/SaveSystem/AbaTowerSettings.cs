using System;

namespace BioTower.SaveData
{
[Serializable]
public class AbaTowerSettings
{
    public int abaMaxInfluenceRadius;
    public int abaMapScale;
    public int abaInfluenceShapeRadius;

    public AbaTowerSettings(float influenceRadius, float mapScale, float shapeRadius)
    {
        SetAbaTowerInfluence(influenceRadius, mapScale, shapeRadius);
    }

    public void SetAbaTowerInfluence(float influenceRadius, float mapScale, float shapeRadius)
    {
        //Debug.Log((int) (influenceRadius * 1000));
        this.abaMaxInfluenceRadius = (int) (influenceRadius * 1000);
        this.abaMapScale = (int) (mapScale * 1000);
        this.abaInfluenceShapeRadius = (int) (shapeRadius * 1000);
    }
}
}