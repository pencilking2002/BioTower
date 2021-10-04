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
    public List<PooledObject> pooledObjects;
    [SerializeField] private GameObject lightFragPrefab;
    [SerializeField] private Vector2 offscreenLocation;


    public PooledObject GetPooledObject(PoolObjectType objectType)
    {
        PooledObject pooledObject = null;
        int indexToRemove = -1;
        for(int i=0; i<pooledObjects.Count; i++)
        {
            PooledObject obj = pooledObjects[i];
            if (obj.objectType == objectType && !obj.isActive)
            {
                pooledObject = obj;
                pooledObject.isActive = true;
                indexToRemove = i;
                break;
            }
        }

        if (indexToRemove != -1)
            pooledObjects.RemoveAt(indexToRemove);

        else if (pooledObject == null)
        {
            pooledObject = CreatePrefab(objectType);
            pooledObject.isActive = true;
            pooledObjects.Add(pooledObject);
        }
        return pooledObject;
    }

    public void AddPooledObject(PooledObject pooledObj)
    {
        if (!pooledObjects.Contains(pooledObj))
        {
            pooledObjects.Add(pooledObj);
            pooledObj.isActive = false;
            pooledObj.transform.position = offscreenLocation;
        }
    }

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