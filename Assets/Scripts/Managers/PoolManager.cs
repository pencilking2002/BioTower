using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace BioTower
{
public enum PoolObjectType
{
    NONE,
    LIGHT_FRAGMENT,
    ITEM_HIGHLIGHT
}

public class PoolManager : MonoBehaviour
{
    [SerializeField] private GameObject lightFragPrefab;
    [SerializeField] private GameObject itemHighlightPrefab;
    [SerializeField] private Vector2 offscreenLocation;
    [SerializeField] private int numObjectsToCreateOnAwake = 10;
    public List<PooledObject> pooledObjects;

    private void Awake()
    {
        CreateObjectsOnAwake(PoolObjectType.LIGHT_FRAGMENT, numObjectsToCreateOnAwake);
        CreateObjectsOnAwake(PoolObjectType.ITEM_HIGHLIGHT, 4);
    }

    private void CreateObjectsOnAwake(PoolObjectType objectType, int numObjects)
    {
        for (int i=0; i<numObjects; i++)
        {
            var obj = CreatePrefab(objectType);
            AddPooledObject(obj);
        }
    }

    public PooledObject GetPooledObject(PoolObjectType objectType)
    {
        PooledObject pooledObject = null;
        bool didFindObject = false;
        for(int i=0; i<pooledObjects.Count; i++)
        {
            PooledObject obj = pooledObjects[i];
            if (obj.objectType == objectType)
            {
                pooledObject = obj;
                //pooledObject.isActive = true;
                RemovePooledObject(pooledObject);
                didFindObject = true;
                break;
            }
        }

        if (!didFindObject)
        {
            pooledObject = CreatePrefab(objectType);
            //pooledObject.isActive = true;
        }
        Debug.Log("Get object from pool");
        return pooledObject;
    }

    public void AddPooledObject(PooledObject pooledObj)
    {
        if (!pooledObjects.Contains(pooledObj))
        {
            pooledObjects.Add(pooledObj);
            //pooledObj.isActive = false;
            pooledObj.transform.SetParent(this.transform, false);
            pooledObj.transform.position = offscreenLocation;
        }
    }

    public void RemovePooledObject(PooledObject obj)
    {
        obj.transform.SetParent(null, false);
        if (pooledObjects.Contains(obj))
            pooledObjects.Remove(obj);
    }

    //public void RemovePooledObject(P)

    private PooledObject CreatePrefab(PoolObjectType objType)
    {
        PooledObject obj = null;
        switch(objType)
        {
            case PoolObjectType.LIGHT_FRAGMENT:
                obj = Instantiate(lightFragPrefab).GetComponent<PooledObject>();
                break;
            case PoolObjectType.ITEM_HIGHLIGHT:
                obj = Instantiate(itemHighlightPrefab).GetComponent<PooledObject>();
                break;
        }
        return obj;
    }

    public PooledObject SpawnItemHighlight(Vector3 worldPos, Vector2 offset)
    {
        var item = GetPooledObject(PoolObjectType.ITEM_HIGHLIGHT);
        var rt = item.GetComponent<RectTransform>();
        rt.SetParent(GameManager.Instance.currTutCanvas.itemHighlightPanel, false);
        Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(Camera.main, worldPos);
        rt.anchoredPosition = screenPoint - GameManager.Instance.currTutCanvas.GetComponent<RectTransform>().sizeDelta / 2f; 
        rt.anchoredPosition += offset;
        rt.GetComponent<ItemHighlight>().Oscillate();
        return item;
    }

    public void DespawnItemHighlight(PooledObject pooledObject)
    {
        pooledObject.GetComponent<ItemHighlight>().Stop(() => {
            AddPooledObject(pooledObject);
        });
    }
}
}