﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace BioTower
{
    public enum PoolObjectType
    {
        NONE, LIGHT_FRAGMENT,
        ITEM_HIGHLIGHT, WAVE_WARNING
    }

    public class PoolManager : MonoBehaviour
    {
        [SerializeField] private GameObject waveWarningPrefab;
        [SerializeField] private GameObject lightFragPrefab;
        [SerializeField] private GameObject itemHighlightPrefab;
        [SerializeField] private int numLightFragmentsToCreate = 10;
        [SerializeField] private int numArrowsToCreate = 4;
        [SerializeField] private int numWaveWarningsToCreate = 3;
        public List<PooledObject> pooledObjects;
        private Vector2 offscreenLocation = new Vector2(100000, 100000);


        private void CreateObjectsOnAwake(PoolObjectType objectType, int numObjects)
        {
            for (int i = 0; i < numObjects; i++)
            {
                var obj = CreatePrefab(objectType);
                AddPooledObject(obj);
            }
        }

        public PooledObject GetPooledObject(PoolObjectType objectType)
        {
            PooledObject pooledObject = null;
            bool didFindObject = false;
            for (int i = 0; i < pooledObjects.Count; i++)
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
            //Debug.Log("Get object from pool");
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
            switch (objType)
            {
                case PoolObjectType.LIGHT_FRAGMENT:
                    obj = Instantiate(lightFragPrefab).GetComponent<PooledObject>();
                    break;
                case PoolObjectType.ITEM_HIGHLIGHT:
                    obj = Instantiate(itemHighlightPrefab).GetComponent<PooledObject>();
                    break;
                case PoolObjectType.WAVE_WARNING:
                    obj = Instantiate(waveWarningPrefab).GetComponent<PooledObject>();
                    break;
            }
            return obj;
        }

        private PooledObject[] GetCurrentItemHighlights()
        {
            var panel = Util.tutCanvas.itemHighlightPanel;
            var itemHighlights = panel.GetComponentsInChildren<PooledObject>();
            return itemHighlights;
        }

        public void DespawnAllitemHighlights()
        {
            var itemHighlights = GetCurrentItemHighlights();
            foreach (PooledObject obj in itemHighlights)
                DespawnItemHighlight(obj);
        }

        public PooledObject SpawnItemHighlight(Vector3 worldPos, Vector2 offset)
        {
            var item = GetPooledObject(PoolObjectType.ITEM_HIGHLIGHT);
            var rt = item.GetComponent<RectTransform>();
            rt.SetParent(Util.tutCanvas.itemHighlightPanel, false);
            Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(Camera.main, worldPos);
            rt.anchoredPosition = screenPoint - Util.tutCanvas.GetComponent<RectTransform>().sizeDelta / 2f;
            rt.anchoredPosition += offset;
            rt.GetComponent<ItemHighlight>().Oscillate();
            return item;
        }

        public void DespawnItemHighlight(PooledObject pooledObject)
        {
            pooledObject.GetComponent<ItemHighlight>().Stop(() =>
            {
                AddPooledObject(pooledObject);
            });
        }

        private void OnLevelAwake(LevelType levelType)
        {
            CreateObjectsOnAwake(PoolObjectType.LIGHT_FRAGMENT, numLightFragmentsToCreate);
            CreateObjectsOnAwake(PoolObjectType.ITEM_HIGHLIGHT, numArrowsToCreate);
            CreateObjectsOnAwake(PoolObjectType.WAVE_WARNING, numWaveWarningsToCreate);
        }

        private void OnEnable()
        {
            EventManager.Game.onLevelAwake += OnLevelAwake;
        }

        private void OnDisable()
        {
            EventManager.Game.onLevelAwake -= OnLevelAwake;
        }
    }
}