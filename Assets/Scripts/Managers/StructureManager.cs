using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BioTower.Structures
{
    public class StructureManager : MonoBehaviour
    {
        public List<Structure> structureList = new List<Structure>();
        [SerializeField] private List<StructureSocket> socketList = new List<StructureSocket>();
        [SerializeField] private float declineDelay = 3;

        private void Update()
        {
            if (GameManager.Instance.gameStates.gameState != GameState.GAME)
                return;

            //(Structure structure in structureList)
            for (int i = 0; i < structureList.Count; i++)
            {
                Structure structure = structureList[i];

                if (structure == null)
                    continue;

                if (!structure.isAlive)
                    continue;

                if (!structure.gameObject.activeInHierarchy)
                    continue;

                DoHealthDeclineOrHeal(structure);
                structure.OnUpdate();
            }
        }

        public bool HasAvailableSockets()
        {
            bool hasFreesockets = false;
            for (int i = 0; i < socketList.Count; i++)
            {
                if (!socketList[i].HasStructure())
                {
                    hasFreesockets = true;
                    break;
                }
            }
            return hasFreesockets;
        }

        private void DoHealthDeclineOrHeal(Structure structure)
        {
            if (structure.structureType == StructureType.ROAD_BARRIER)
                return;

            bool healEnabled = RandomHealEnabled(structure.structureType);

            if (Util.gameSettings.upgradeSettings.enableTowerHealthDecline)
            {
                if (Time.time > structure.lastDeclineTime + declineDelay)
                {
                    structure.lastDeclineTime = Time.time;
                    float healChance = GetRandomFloat();
                    bool isHeal = healChance > 1 - Util.gameSettings.randomHealChance && healEnabled;

                    if (isHeal && healEnabled)
                        structure.GainHealth(Util.gameSettings.randomHealAmount);
                    else
                        structure.TakeDamage(Util.gameSettings.declineDamage);
                }
            }
        }

        private bool RandomHealEnabled(StructureType structureType)
        {
            if (structureType == StructureType.ABA_TOWER && Util.upgradeSettings.enableAbaTowerRandomHeal)
                return true;
            else if (structureType == StructureType.CHLOROPLAST && Util.upgradeSettings.enableChloroTowerRandomHeal)
                return true;
            else
                return false;
        }

        private float GetRandomFloat()
        {
            float healChance = 0;
            if (!Util.upgradeSettings.enableAbaTowerRandomHeal)
                return healChance;

            healChance = UnityEngine.Random.Range(0.0f, 1.0f);
            return healChance;

        }

        private void OnStructureCreated(Structure structure, bool doSquishyAnim)
        {
            if (structure.structureType == StructureType.DNA_BASE)
                return;

            structureList.Add(structure);
            //Debug.Log("Structure created ev recieved");
        }

        private void OnStructureDestroyed(Structure structure)
        {
            structureList.Remove(structure);
        }

        private void OnSocketStart(StructureSocket socket)
        {
            if (!socketList.Contains(socket))
                socketList.Add(socket);
        }

        private void OnGameOver(bool isWin, float delay)
        {
            structureList.Clear();
            socketList.Clear();
        }

        private void OnStructureActivated(Structure tower)
        {
            if (tower.IsMiniChloroTower())
            {
                structureList.Add(tower);
            }
        }

        private void OnWaveStateInit(WaveMode waveMode)
        {
            if (waveMode == WaveMode.ENDED && !Util.waveManager.currWave.IsFinalWave())
            {
                foreach (Structure tower in structureList)
                {
                    if (tower.IsMiniChloroTower())
                    {
                        var miniTower = (MiniChloroplastTower)tower;
                        miniTower.ShootFragment(8);
                        return;
                    }
                }
            }
        }

        private void OnEnable()
        {
            EventManager.Structures.onStructureActivated += OnStructureActivated;
            EventManager.Structures.onStructureCreated += OnStructureCreated;
            EventManager.Structures.onStructureDestroyed += OnStructureDestroyed;
            EventManager.Structures.onSocketStart += OnSocketStart;
            EventManager.Game.onGameOver += OnGameOver;
            EventManager.Wave.onWaveStateInit += OnWaveStateInit;
        }

        private void OnDisable()
        {
            EventManager.Structures.onStructureActivated -= OnStructureActivated;
            EventManager.Structures.onStructureCreated -= OnStructureCreated;
            EventManager.Structures.onStructureDestroyed -= OnStructureDestroyed;
            EventManager.Structures.onSocketStart -= OnSocketStart;
            EventManager.Game.onGameOver -= OnGameOver;
            EventManager.Wave.onWaveStateInit -= OnWaveStateInit;
        }
    }
}