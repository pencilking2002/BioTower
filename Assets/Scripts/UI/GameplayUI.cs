using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using BioTower.Structures;
using BioTower.SaveData;
using TMPro;

namespace BioTower.UI
{
public class GameplayUI : MonoBehaviour
{
    public CanvasGroup gameUIPanel;
    [SerializeField] private Button AbaTowerButton;
    [SerializeField] private Button Pp2cTowerButton;
    [SerializeField] private Button chloroplastTowerButton;
    [SerializeField] private Button mitoTowerButton;
    [SerializeField] private Button currSelectedBtn;
    [SerializeField] private TextMeshProUGUI playerCurrencyText;
    private Dictionary<StructureType,Button> towerButtonMap = new Dictionary<StructureType, Button>();


    private void Awake()
    {
        towerButtonMap.Add(StructureType.ABA_TOWER, AbaTowerButton);
        towerButtonMap.Add(StructureType.PPC2_TOWER, Pp2cTowerButton);
        towerButtonMap.Add(StructureType.CHLOROPLAST, chloroplastTowerButton);
        towerButtonMap.Add(StructureType.MITOCHONDRIA, mitoTowerButton);
    }

    private void Start()
    {
        SetTowerPrice(StructureType.ABA_TOWER);
        SetTowerPrice(StructureType.PPC2_TOWER);
        SetTowerPrice(StructureType.CHLOROPLAST);
        SetTowerPrice(StructureType.MITOCHONDRIA);
    }

    ///<Summary>
    /// Sets the tower price on the button
    ///</Summary>
    private void SetTowerPrice(StructureType structureType)
    {
        var text = towerButtonMap[structureType].transform.Find("Panel").Find("PriceText").GetComponent<Text>();
        int cost = Util.gameSettings.GetTowerCost(structureType);
        text.text = cost.ToString();
    }

    private void Update()
    {
        playerCurrencyText.text = GameManager.Instance.econManager.playerCurrency.ToString();
    }


    // BUTTON METHODS -------------------------------------------------------------------------------
    
    public void OnPressAbaTowerButton()
    {
        // Make sure you can't press the button if there's currently a tutorial displayed that's
        // not relevant to the button
        if (TutorialCanvas.tutorialInProgress)
        {
            var requiredAction = GameManager.Instance.currTutCanvas.currTutorial.requiredAction;
            if (requiredAction != RequiredAction.TAP_ABA_TOWER_BUTTON)
                return;
        }
        bool canBuildTower = CooldownManager.structureCooldownMap[StructureType.ABA_TOWER];
        if (canBuildTower)
        {
            if (currSelectedBtn != AbaTowerButton.GetComponent<Button>())
                AnimateButton(AbaTowerButton);

            currSelectedBtn = AbaTowerButton;
            EventManager.UI.onPressTowerButton?.Invoke(StructureType.ABA_TOWER);
            EventManager.UI.onTapButton?.Invoke();
        }
    }
    
    public void OnPressPPC2TowerButton()
    {
        bool canBuildTower = CooldownManager.structureCooldownMap[StructureType.PPC2_TOWER];
        if (canBuildTower)
        {
            AnimateButton(Pp2cTowerButton);
            currSelectedBtn = Pp2cTowerButton;
            EventManager.UI.onPressTowerButton?.Invoke(StructureType.PPC2_TOWER);
            EventManager.UI.onTapButton?.Invoke();
        }
    }

    public void OnPressChloroplastButton()
    {
        bool canBuildTower = CooldownManager.structureCooldownMap[StructureType.CHLOROPLAST];
        if (canBuildTower)
        {
            AnimateButton(chloroplastTowerButton);
            currSelectedBtn = chloroplastTowerButton;
            EventManager.UI.onPressTowerButton?.Invoke(StructureType.CHLOROPLAST);
            EventManager.UI.onTapButton?.Invoke();
        }
    }

    public void OnPressMitoButton()
    {
        bool canBuildTower = CooldownManager.structureCooldownMap[StructureType.MITOCHONDRIA];
        if (canBuildTower)
        {
            AnimateButton(mitoTowerButton);
            currSelectedBtn = mitoTowerButton;
            EventManager.UI.onPressTowerButton?.Invoke(StructureType.MITOCHONDRIA);
            EventManager.UI.onTapButton?.Invoke();
        }
    }

    private void HandleButtonColor(Button button)
    {
        var image = button.transform.Find("Panel").GetComponent<Image>();
        var oldColor = image.color;
        image.color = Color.grey;
        LeanTween.delayedCall(gameObject, GameManager.Instance.cooldownManager.structureSpawnCooldown, () => {
            image.color = oldColor;
        });
    }

    private void OnSpendCurrency(int numSpent, int currTotal)
    {
        //playerCurrencyText.text = currTotal.ToString();
        var oldScale = playerCurrencyText.transform.localScale;
        LeanTween.scale(playerCurrencyText.gameObject, oldScale * 1.1f, 0.1f).setLoopPingPong(1);
    }

    private void OnGainCurrency(int numGained, int currTotal)
    {
        //playerCurrencyText.text = currTotal.ToString();
        Debug.Log($"Currency gained");
    }

    private void OnStructureCooldownStarted(StructureType structureType, float cooldown)
    {
        if (!towerButtonMap.ContainsKey(structureType))
            return;
        
        DeselectCurrentButton();

        var button = towerButtonMap[structureType];

        button.interactable = false;
        var cooldownImage = button.transform.Find("Cooldown").GetComponent<Image>();
        LeanTween.value(1, 0, cooldown).setOnUpdate((float val) => {
            cooldownImage.fillAmount = val;
        })
        .setOnComplete(() => {
            button.interactable = true;
        });

        HandleButtonColor(button);
    }   

    private void AnimateButton(Button button)
    {
        var initPos = button.transform.localPosition;
        if (currSelectedBtn != null)
            LeanTween.moveLocalY(currSelectedBtn.gameObject, initPos.y, 0.1f);

        LeanTween.moveLocalY(button.gameObject, initPos.y + 20, 0.1f);
    }

    private void DeselectCurrentButton()
    {
         // Deselect current button
        if (currSelectedBtn != null)
        {
            LeanTween.moveLocalY(currSelectedBtn.gameObject, currSelectedBtn.transform.localPosition.y-20, 0.1f);
            currSelectedBtn = null;
        }
    }

    private void OnLevelAwake(LevelType levelType)
    {
        if (levelType == LevelType.LEVEL_00)
        {
            Pp2cTowerButton.gameObject.SetActive(false);
            chloroplastTowerButton.gameObject.SetActive(false);
            mitoTowerButton.gameObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        EventManager.Game.onLevelAwake += OnLevelAwake;
        EventManager.Game.onSpendCurrency += OnSpendCurrency;
        EventManager.Game.onGainCurrency += OnGainCurrency;
        EventManager.Structures.onStructureCooldownStarted += OnStructureCooldownStarted;
    }

    private void OnDisable()
    {
        EventManager.Game.onLevelAwake -= OnLevelAwake;
        EventManager.Game.onSpendCurrency -= OnSpendCurrency;
        EventManager.Game.onGainCurrency -= OnGainCurrency;
        EventManager.Structures.onStructureCooldownStarted -= OnStructureCooldownStarted;
    }
}
}
