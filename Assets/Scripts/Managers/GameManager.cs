using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections.Generic;
using BioTower.Units;
using BioTower.Structures;
using PolyNav;
using BioTower.Level;
using Sirenix.OdinInspector;
using BioTower.SaveData;

namespace BioTower
{

    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;
        public GameStates gameStates;
        public UpgradeTree upgradeTree;
        public UpgradeTextData upgradeTextData;
        public Params upgradeSettings;
        public GameSettings gameSettings;


        [Header("Scene References")]
        public LevelMap levelMap;
        public DNABase playerBase;
        [SerializeField] private List<EnemyUnit> enemyList = new List<EnemyUnit>();
        private GameObject enemyContainer;


        [Header("References")]
        public BombPanel bombPanel;
        public CamController cam;
        public ObjectShake objectShake;
        public UnitManager unitManager;
        //public TutorialCanvas currTutCanvas;
        public Util util;
        public WaveManager waveManager;
        public ColorManager colorManager;
        public PlacementManager placementManager;
        public TapManager tapManager;
        public EconomyManager econManager;
        public CooldownManager cooldownManager;
        public SaveSystem saveManager;
        private WaypointManager waypointManager;
        public CrystalManager crystalManager;
        public PoolManager poolManager;
        public StructureManager structureManager;
        private Transform _projectilesContainer;
        public Transform projectilesContainer
        {
            get
            {
                if (_projectilesContainer == null)
                    _projectilesContainer = GameObject.FindGameObjectWithTag(Constants.projectilesContainer).transform;
                return _projectilesContainer;
            }
        }

        private BootController _bootController;
        public BootController bootController
        {
            get
            {
                if (_bootController == null)
                    _bootController = GameObject.FindGameObjectWithTag("BootController").GetComponent<BootController>();
                return _bootController;
            }
        }

        [TabGroup("Particle Prefabs")] public GameObject towerDeathExplosionPrefab;
        [TabGroup("Particle Prefabs")] public GameObject crystalExplosionPrefab;

        private void Awake()
        {
            InitInstance();
        }

        private void Update()
        {
            //Debug.Log("YO");

            if (Input.GetKeyDown(KeyCode.R))
            {
                LoadLevel(0);
            }

            if (Input.GetKeyDown(KeyCode.H))
            {
                playerBase.TakeDamage(1);
                Debug.Log("Make player base take 100 DMG");
            }

            if (GameManager.Instance.gameStates.IsGameState())
            {
                if (Input.GetKeyDown(KeyCode.W))
                    EventManager.Game.onGameOver?.Invoke(true, 0);
                else if (Input.GetKeyDown(KeyCode.L))
                    EventManager.Game.onGameOver?.Invoke(false, 0);

                if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    GameManager.Instance.econManager.GainCurrency(10);
                }

                if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    GameManager.Instance.econManager.SpendCurrency(10);
                }
            }

            //Time.timeScale = gameSettings.timeScale;  
        }

        public void CreateTowerExplosion(Vector3 pos)
        {
            GameObject explosion = Instantiate(towerDeathExplosionPrefab);
            explosion.transform.position = pos;
            Destroy(explosion, 2);
        }

        public void CreateCrystalExplosion(Vector3 pos)
        {
            GameObject explosion = Instantiate(crystalExplosionPrefab);
            explosion.transform.position = pos;
            Destroy(explosion, 2);
        }

        public void CreateLightExplosion(Vector3 pos)
        {
            GameObject explosion = Instantiate(crystalExplosionPrefab);
            explosion.transform.position = pos;
            Destroy(explosion, 2);
        }

        public WaypointManager GetWaypointManager()
        {
            if (!waypointManager)
                waypointManager = GameObject.FindGameObjectWithTag(Constants.waypointManager).GetComponent<WaypointManager>();

            return waypointManager;
        }

        public void RegisterMap(LevelMap levelMap)
        {
            this.levelMap = levelMap;
        }

        /// <summary>
        /// Registers enemy in list and 
        /// sets player base as the target
        /// </summary>
        /// <param name="enemy"></param>
        public void RegisterEnemy(EnemyUnit enemy)
        {
            if (enemyList.Contains(enemy) || playerBase == null)
                return;

            enemy.transform.parent = enemyContainer.transform;
            enemyList.Add(enemy);
            enemy.agent.map = levelMap.map;
            //        Debug.Log("Register enemy agent: " + enemy.gameObject.name + ". map: " + levelMap.map.gameObject.name + ". destination: " + playerBase.gameObject.name);
        }

        public void UnregisterEnemy(EnemyUnit enemy)
        {
            enemy.StopMoving();
            enemyList.Remove(enemy);
            //Debug.Log("Unregister enemy");
        }

        public void RegisterPlayerBase(DNABase playerbase)
        {
            playerBase = playerbase;
        }

        public void LoadLevel(int sceneIndex)
        {
            for (int i = 0; i < enemyList.Count; i++)
                UnregisterEnemy(enemyList[i]);

            enemyContainer = null;
            playerBase = null;
            SceneManager.LoadScene(sceneIndex);
        }


        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            //Debug.Log($"Scene: {scene}. Mode: {mode}");
            enemyContainer = GameObject.FindGameObjectWithTag(Constants.enemyContainer);
            enemyList = new List<EnemyUnit>();

            EventManager.Game.onLevelLoaded_01?.Invoke();
            EventManager.Game.onLevelLoaded_02?.Invoke();
        }

        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private void InitInstance()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(transform.parent.gameObject);
            }
            else
            {
                Destroy(gameObject);
                Destroy(this);
            }
        }
    }
}
