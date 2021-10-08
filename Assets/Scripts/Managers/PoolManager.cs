using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace BioTower
{
public enum PoolObjectType
{
    NONE,
    LIGHT_FRAGMENT
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
        CreateObjectsOnAwake(numObjectsToCreateOnAwake);
    }

    private void CreateObjectsOnAwake(int numObjects)
    {
        for (int i=0; i<numObjects; i++)
        {
            var obj = CreatePrefab(PoolObjectType.LIGHT_FRAGMENT);
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
        }
        return obj;
    }
}
}