﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace BioTower.Level
{
    public enum WaypointType
    {
        DEFAULT,
        FORK,
        SPAWN_POINT,
        END_POINT
    }
    public class Waypoint : MonoBehaviour
    {
        public WaypointType waypointType;
        [HideIf("isEndpoint")] public Waypoint nextWaypoint;

        [ShowIf("isFork")]
        public Waypoint nextWaypoint_02;
        public bool isFork => waypointType == WaypointType.FORK;
        public bool isSpawnPoint => waypointType == WaypointType.SPAWN_POINT;
        public bool isEndpoint => waypointType == WaypointType.END_POINT;
        private WaveWarning waveWarning;

        public Waypoint ChooseNextWaypoint()
        {
            if (nextWaypoint_02)
            {
                int randIndex = UnityEngine.Random.Range(0, 2);
                return randIndex == 0 ? nextWaypoint : nextWaypoint_02;
            }
            else
            {
                return nextWaypoint;
            }
        }

        private Vector3 GetTangent(Vector3 normal)
        {
            Vector3 tangent;
            Vector3 t1 = Vector3.Cross(normal, Vector3.up);
            Vector3 t2 = Vector3.Cross(normal, Vector3.forward);

            tangent = t1.sqrMagnitude > t1.sqrMagnitude ? t1 : t2;
            return tangent;
        }

        private void OnDisplayWaveWarning(Waypoint waypoint)
        {
            if (waypoint == this && waypointType == WaypointType.SPAWN_POINT)
            {
                var pooledObj = Util.poolManager.GetPooledObject(PoolObjectType.WAVE_WARNING);
                pooledObj.transform.position = this.transform.position;
                pooledObj.transform.right = this.transform.right;
                pooledObj.GetWaveWarning().Animate();
                waveWarning = pooledObj.GetWaveWarning();
            }
        }

        private void OnStopWaveWarning(Waypoint waypoint)
        {
            if (waypoint == this && waypointType == WaypointType.SPAWN_POINT)
            {
                if (waveWarning)
                {
                    waveWarning.FadeOut();
                    LeanTween.delayedCall(gameObject, 0.5f, () =>
                    {
                        Util.poolManager.AddPooledObject(waveWarning.GetComponent<PooledObject>());
                    });
                }
            }
        }
        private void OnEnable()
        {
            EventManager.Wave.onDisplayWaveWarning += OnDisplayWaveWarning;
            EventManager.Wave.onStopWaveWarning += OnStopWaveWarning;

        }

        private void OnDisable()
        {
            EventManager.Wave.onDisplayWaveWarning -= OnDisplayWaveWarning;
            EventManager.Wave.onStopWaveWarning -= OnStopWaveWarning;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = isFork ? Color.yellow : Color.white;

            if (nextWaypoint != null)
            {
                Gizmos.DrawLine(transform.position, nextWaypoint.transform.position);

                var arrowDir = (transform.position - nextWaypoint.transform.position).normalized * 0.5f;
                var tangent = GetTangent(arrowDir) * 0.5f;
                Gizmos.DrawLine(nextWaypoint.transform.position, nextWaypoint.transform.position + tangent + arrowDir);
                Gizmos.DrawLine(nextWaypoint.transform.position, nextWaypoint.transform.position - tangent + arrowDir);

            }

            if (isFork && nextWaypoint_02 != null)
            {
                Gizmos.DrawLine(transform.position, nextWaypoint_02.transform.position);

                var arrowDir = (transform.position - nextWaypoint_02.transform.position).normalized * 0.5f;
                var tangent = GetTangent(arrowDir) * 0.5f;
                Gizmos.DrawLine(nextWaypoint_02.transform.position, nextWaypoint_02.transform.position + tangent + arrowDir);
                Gizmos.DrawLine(nextWaypoint_02.transform.position, nextWaypoint_02.transform.position - tangent + arrowDir);

            }

            if (isSpawnPoint)
                Gizmos.color = Color.blue;
            else if (isEndpoint)
                Gizmos.color = Color.green;
            else if (isFork)
                Gizmos.color = Color.yellow;
            else
                Gizmos.color = Color.red;

            Gizmos.DrawSphere(transform.position, 0.2f);
        }
    }
}