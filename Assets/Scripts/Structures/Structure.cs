using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BioTower.Units;
using Sirenix.OdinInspector;
using Shapes;
using BioTower.UI;

namespace BioTower.Structures
{
    public enum StructureState
    {
        NONE, ACTIVE,
        DESTROYED
    }

    public enum StructureType
    {
        ABA_TOWER, DNA_BASE,
        NONE, PPC2_TOWER,
        CHLOROPLAST, MITOCHONDRIA,
        ROAD_BARRIER, MINI_CHLOROPLAST_TOWER
    }

    public class Structure : MonoBehaviour
    {
        [ReadOnly][SerializeField] protected StructureSocket socket;
        public StructureType structureType;
        [SerializeField] public bool hasHealth;
        [ShowIf("hasHealth")] public bool isAlive = true;
        [ShowIf("hasHealth")][Range(0, 100)][SerializeField] protected int maxHealth;
        [ShowIf("hasHealth")][SerializeField] protected int currHealth;
        protected HealthBar healthBar;
        [SerializeField] protected GameObject spriteOutline;
        public SpriteRenderer sr;
        [HideInInspector] public float lastDeclineTime;
        [SerializeField] protected Disc influenceVisuals;
        [SerializeField] protected TrailRenderer unitSpawnTrail;
        public List<Unit> units;
        public TowerAlert towerAlert;
        private Vector3 initSpriteScale;
        private Vector3 initLocalSpritePos;

        public virtual void Awake()
        {
            healthBar = GetComponentInChildren<HealthBar>();
            initSpriteScale = sr.transform.localScale;
            initLocalSpritePos = sr.transform.localPosition;
            lastDeclineTime = Time.time;
        }

        public virtual void Init(StructureSocket socket)
        {
            currHealth = maxHealth;

            if (hasHealth)
            {
                healthBar.Init(currHealth);
            }

            if (!IsBarrier() && !IsMiniChloroTower())
            {
                GameManager.Instance.tapManager.selectedStructure = this;
                GameManager.Instance.tapManager.hasSelectedStructure = true;
                this.socket = socket;
                AnimateTowerDrop();
            }
        }

        public virtual void AnimateTowerDrop()
        {
            var healthBarScaleX = healthBar.transform.localScale.x;
            var healthBarScaleY = healthBar.transform.localScale.y;
            healthBar.transform.localScale = Vector3.zero;
            sr.transform.localScale = Vector3.zero;


            var initPos = initLocalSpritePos;
            //var dropPosition = initPos + new Vector3(0, 25, 0);
            //sr.transform.localPosition = dropPosition;

            var seq = LeanTween.sequence();

            // seq.append(() =>
            // {
            //     LeanTween.moveLocal(sr.gameObject, initPos, 0.3f);
            //     LeanTween.scale(sr.gameObject, initSpriteScale + new Vector3(0, 0.5f, 0), 0.3f);
            // });

            // seq.append(0.31f);
            // var scale = initSpriteScale;
            // scale.x *= 2f;
            // scale.y *= 0.5f;
            // if (socket != null)
            //     seq.append(socket.Jiggle);

            // seq.append(LeanTween.scale(sr.gameObject, scale, 0.1f));
            // seq.append(() =>
            // {
            //     bool isFirstLevelAndIsPlayerBase = LevelInfo.current.IsFirstLevel() && IsPlayerBase();

            //     if (!isFirstLevelAndIsPlayerBase)
            //         Util.objectShake.Shake(GameManager.Instance.cam.gameObject, 0.4f, 0.1f);
            // });
            // seq.append(LeanTween.scale(sr.gameObject, initSpriteScale, 0.25f).setEaseOutExpo());
            // seq.append(() => { LeanTween.scaleX(healthBar.gameObject, healthBarScaleX, 0.4f).setEaseOutElastic(); });
            // seq.append(() => { LeanTween.scaleY(healthBar.gameObject, healthBarScaleY, 0.7f).setEaseOutElastic(); });

            //seq.append(LeanTween.scale(sr.gameObject, initSpriteScale, 0.25f).setEaseOutBack());

            seq.append(LeanTween.scale(sr.gameObject, initSpriteScale, 0.25f).setEaseOutBack());

            seq.append(() =>
            {
                bool isFirstLevelAndIsPlayerBase = LevelInfo.current.IsFirstLevel() && IsPlayerBase();

                if (!isFirstLevelAndIsPlayerBase)
                    Util.objectShake.Shake(GameManager.Instance.cam.gameObject, 0.3f, 0.05f);
            });

            seq.append(() => { LeanTween.scaleX(healthBar.gameObject, healthBarScaleX, 0.4f).setEaseOutElastic(); });
            seq.append(() => { LeanTween.scaleY(healthBar.gameObject, healthBarScaleY, 0.7f).setEaseOutElastic(); });

            seq.append(() =>
            {
                EventManager.Structures.onStructureCreated?.Invoke(this, false);
            });

        }

        public virtual void OnUpdate() { }
        public virtual int GetCurrHealth() { return currHealth; }
        public virtual int GetMaxHealth() { return maxHealth; }

        public virtual bool IsMaxHealth() { return currHealth == maxHealth; }

        protected Unit GetClosestUnit(EnemyUnit enemy, out bool unitFound)
        {
            unitFound = false;
            float furthestDistance = Mathf.Infinity;
            Unit closestUnit = null;

            foreach (var unit in units)
            {
                var distance = Vector2.Distance(unit.transform.position, enemy.transform.position);
                if (distance < furthestDistance)
                {
                    furthestDistance = distance;
                    closestUnit = unit;
                    unitFound = true;
                }
            }
            return closestUnit;
        }

        public virtual void TakeDamage(int numDamage)
        {
            if (GameManager.Instance.gameStates.IsGameState() && hasHealth && isAlive)
            {
                if (this == null)
                    return;

                currHealth -= numDamage;
                currHealth = Mathf.Clamp(currHealth, 0, maxHealth);
                healthBar.SetHealth(currHealth);
                // LeanTween.value(gameObject, healthBar.value, currHealth, 0.25f)
                // .setOnUpdate((float val) =>
                // {
                //     healthBar.value = val;
                // });

                EventManager.Structures.onStructureLoseHealth?.Invoke(this);

                if (currHealth == 0)
                    KillStructure();
                else
                    DoHealthVisual(new Color(1, 0.5f, 0.5f, 1), 1.01f);
            }
        }

        public virtual void GainHealth(int numHealth)
        {
            currHealth += numHealth;
            currHealth = Mathf.Clamp(currHealth, 0, maxHealth);
            healthBar.SetHealth(currHealth);
            DoHealthVisual(Color.green);
            EventManager.Structures.onStructureGainHealth?.Invoke(this);
        }

        private void DoHealthVisual(Color targetColor, float scaleUpFactor = 1.1f)
        {
            if (this == null)
                return;

            // Reset tower before applying tweens to it
            LeanTween.cancel(sr.gameObject);
            Debug.Log("Do death visual");
            sr.transform.localScale = initSpriteScale;
            sr.color = Color.white;

            Util.ScaleBounceSprite(sr, scaleUpFactor);
            var oldColor = sr.color;
            sr.color = targetColor;
            LeanTween.value(sr.gameObject, sr.color, oldColor, 0.25f)
            .setOnUpdate((Color col) =>
            {
                sr.color = col;
            });
        }

        public virtual void KillStructure()
        {
            isAlive = false;
            GameManager.Instance.CreateTowerExplosion(transform.position);
            EventManager.Structures.onStructureDestroyed?.Invoke(this);

            if (socket != null)
                socket.SetHasNone();

            Util.objectShake.Shake(GameManager.Instance.cam.gameObject, 0.4f, 0.1f);

            Destroy(gameObject);
        }

        public virtual void OnTapStructure(Vector3 screenPoint) { }
        public virtual void SpawnUnits(int numUnits) { }

        private void SelectStructure(bool doSquishyAnim)
        {
            if (GameManager.Instance == null)
                return;

            if (IsMiniChloroTower())
                return;

            if (spriteOutline != null)
                spriteOutline.SetActive(true);

            if (influenceVisuals != null)
                influenceVisuals.gameObject.SetActive(true);

            if (doSquishyAnim)
                DoSquishyAnimation(initSpriteScale, initSpriteScale);
        }

        private void DeselectStructure()
        {
            if (spriteOutline != null)
                spriteOutline.SetActive(false);

            if (influenceVisuals != null)
                influenceVisuals.gameObject.SetActive(false);
        }

        private void DoSquishyAnimation(Vector3 startingScale, Vector3 targetScale, bool cancelAnim = true)
        {
            Debug.Log("Do squishy animation");

            if (cancelAnim)
                LeanTween.cancel(sr.gameObject);

            sr.transform.localScale = startingScale;
            float randScaleX = UnityEngine.Random.Range(1.2f, 1.4f);
            float randScaleY = UnityEngine.Random.Range(1.3f, 1.5f);
            var newScale = targetScale;
            newScale.x *= randScaleX;
            newScale.y *= randScaleY;

            var seq = LeanTween.sequence();
            seq.append(LeanTween.scale(sr.gameObject, newScale, 0.1f));
            seq.append(LeanTween.scale(sr.gameObject, targetScale, 0.2f));
            //Debug.Log("Do squishy anim");
        }

        public bool IsAbaTower()
        {
            return structureType == StructureType.ABA_TOWER;
        }

        public bool IsPPC2Tower()
        {
            return structureType == StructureType.PPC2_TOWER;
        }

        public bool IsChloroTower()
        {
            return structureType == StructureType.CHLOROPLAST;
        }

        public bool IsMitoTower()
        {
            return structureType == StructureType.MITOCHONDRIA;
        }

        public bool IsMiniChloroTower()
        {
            return structureType == StructureType.MINI_CHLOROPLAST_TOWER;
        }

        public bool IsBarrier()
        {
            return structureType == StructureType.ROAD_BARRIER;
        }

        public bool IsPlayerBase()
        {
            return structureType == StructureType.DNA_BASE;
        }

        public int GetNumUnits()
        {
            return units.Count;
        }

        private void OnPressDestroyTowerBtn(Structure structure)
        {
            if (structure != this)
                return;

            KillStructure();
        }

        public virtual void OnStructureSelected(Structure structure)
        {
            if (structure == this)
                SelectStructure(true);
            else
                DeselectStructure();
        }

        public virtual void OnStructureCreated(Structure structure, bool doSquishyAnim)
        {
            if (structure == this)
                SelectStructure(doSquishyAnim);
            else
                DeselectStructure();
        }

        public virtual void OnEnable()
        {
            EventManager.UI.onPressTowerDestroyedBtn += OnPressDestroyTowerBtn;
            EventManager.Structures.onStructureSelected += OnStructureSelected;
            EventManager.Structures.onStructureCreated += OnStructureCreated;
        }

        public virtual void OnDisable()
        {
            EventManager.UI.onPressTowerDestroyedBtn = OnPressDestroyTowerBtn;
            EventManager.Structures.onStructureSelected -= OnStructureSelected;
            EventManager.Structures.onStructureCreated -= OnStructureCreated;
        }

        private void OnDestroy() { isAlive = false; }
    }
}

