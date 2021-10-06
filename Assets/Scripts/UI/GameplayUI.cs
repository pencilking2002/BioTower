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
    public GameObject currencyContainer;
    [SerializeField] private TextMeshProUGUI playerCurrencyText;
    public Dictionary<StructureType,Button> towerButtonMap = new Dictionary<StructureType, Button>();


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

    public StructureType GetSelectedButtonType()
    {
        foreach(KeyValuePair<StructureType, Button> btn in towerButtonMap)
        {
            if (btn.Value == currSelectedBtn)
                return btn.Key;
        }
        return StructureType.NONE;
    }

    ///<Summary>
    /// Sets the tower price on the button
    ///</Summary>
    private void SetTowerPrice(StructureType structureType)
    {
        var text = towerButtonMap[structureType].transform.Find("Panel").Find("PriceText").GetComponent<Text>();
        int cost = Util.gameSettings.upgradeSettings.GetTowerCost(structureType);
        text.text = cost.ToString();
    }

    private void Update()
    {
        playerCurrencyText.text = GameManager.Instance.econManager.playerCurrency.ToString();
    }


    // BUTTON METHODS -------------------------------------------------------------------------------
    

    public void OnPressAbaTowerButton()
    {
        if (!Util.towerManager.HasAvailableSockets())
        {
            Util.HandleInvalidButtonPress(AbaTowerButton);
            return;
        }   

        if (!InputController.canPressButtons)
        {
            Util.HandleInvalidButtonPress(AbaTowerButton);
            return;
        }

        bool canBuildTower = CooldownManager.structureCooldownMap[StructureType.ABA_TOWER];
        if (!canBuildTower)
            return;
        
        if (GameManager.Instance.econManager.CanBuyTower(StructureType.ABA_TOWER))
        {
            HandleButtonPress(AbaTowerButton, StructureType.ABA_TOWER);
            //Debug.Log("ABA Button press");
        }
        else
        {
            Util.HandleInvalidButtonPress(AbaTowerButton);
            GameManager.Instance.objectShake.ShakeHorizontal(currencyContainer, 0.15f, 5.0f);
        }
    }

    public void OnPressPPC2TowerButton()
    {
        if (!Util.towerManager.HasAvailableSockets())
        {
            Util.HandleInvalidButtonPress(AbaTowerButton);
            return;
        }   

        bool canBuildTower = CooldownManager.structureCooldownMap[StructureType.PPC2_TOWER];
        if (!canBuildTower)
            return;

        if (GameManager.Instance.econManager.CanBuyTower(StructureType.PPC2_TOWER))
        {
            HandleButtonPress(Pp2cTowerButton, StructureType.PPC2_TOWER);
        }
        else
        {
            Util.HandleInvalidButtonPress(Pp2cTowerButton);
            GameManager.Instance.objectShake.ShakeHorizontal(currencyContainer, 0.15f, 5.0f);
        }
    }

    public void OnPressChloroplastButton()
    {
        if (!Util.towerManager.HasAvailableSockets())
        {
            Util.HandleInvalidButtonPress(AbaTowerButton);
            return;
        }   

        bool canBuildTower = CooldownManager.structureCooldownMap[StructureType.CHLOROPLAST];
        if (!canBuildTower)
            return;

        if (GameManager.Instance.econManager.CanBuyTower(StructureType.PPC2_TOWER))
        {
            HandleButtonPress(chloroplastTowerButton, StructureType.CHLOROPLAST);
        }
        else
        {
            Util.HandleInvalidButtonPress(chloroplastTowerButton);
            GameManager.Instance.objectShake.ShakeHorizontal(currencyContainer, 0.15f, 5.0f);
        }
    }

    public void OnPressMitoButton()
    {
        if (!Util.towerManager.HasAvailableSockets())
        {
            Util.HandleInvalidButtonPress(AbaTowerButton);
            return;
        }   
        
        bool canBuildTower = CooldownManager.structureCooldownMap[StructureType.MITOCHONDRIA];
        if (!canBuildTower)
            return;

        if (GameManager.Instance.econManager.CanBuyTower(StructureType.MITOCHONDRIA))
        {
            HandleButtonPress(chloroplastTowerButton, StructureType.MITOCHONDRIA);
        }
        else
        {
            Util.HandleInvalidButtonPress(mitoTowerButton);
            GameManager.Instance.objectShake.ShakeHorizontal(currencyContainer, 0.15f, 5.0f);
        }
    }

    private void HandleButtonPress(Button button, StructureType structureType)
    {
        AnimateButton(button);
        currSelectedBtn = button;
        EventManager.UI.onPressTowerButton?.Invoke(structureType);
        EventManager.UI.onTapButton?.Invoke(true);
    }

    private void AnimateButton(Button button)
    {
        if (button == currSelectedBtn)
            return;
            
        var initPos = button.transform.localPosition;
        if (currSelectedBtn != null)
            LeanTween.moveLocalY(currSelectedBtn.gameObject, initPos.y, 0.1f);

        LeanTween.moveLocalY(button.gameObject, initPos.y + 20, 0.1f);

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

    private void PingPongScaleCurrencyUI(float targetScale)
    {
        var oldScale = playerCurrencyText.transform.localScale;
        LeanTween.scale(playerCurrencyText.gameObject, oldScale * targetScale, 0.1f).setLoopPingPong(1);
    }

    private void OnSpendCurrency(int numSpent, int currTotal)
    {
        PingPongScaleCurrencyUI(1.2f);
    }

    private void OnGainCurrency(int numGained, int currTotal)
    {
        //Debug.Log($"Currency gained");
    }

    private void OnStructureCooldownStarted(StructureType structureType, float cooldown)
    {
        if (!towerButtonMap.ContainsKey(structureType))
            return;
        
        DeselectCurrentButton();

        var button = towerButtonMap[structureType];

        button.interactable = false;
        var cooldownImage = button.transform.Find("Cooldown").GetComponent<Image>();
        LeanTween.value(gameObject, 1, 0, cooldown).setOnUpdate((float val) => {
            cooldownImage.fillAmount = val;
        })
        .setOnComplete(() => {
            button.interactable = true;
        });

        HandleButtonColor(button);
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

    private void OnLevelStart(LevelType levelType)
    {
        // if (levelType == LevelType.LEVEL_01)
        // {
        //     Pp2cTowerButton.gameObject.SetActive(false);
        //     chloroplastTowerButton.gameObject.SetActive(false);
        //     mitoTowerButton.gameObject.SetActive(false);
        //     //Debug.Log("Level Awake")
        // }
        bool ppc2TowerUnlocked = Util.upgradeSettings.ppc2TowerUnlocked;
        Pp2cTowerButton.gameObject.SetActive(ppc2TowerUnlocked);

        bool chloroTowerUnlocked = Util.upgradeSettings.chloroTowerUnlocked;
        chloroplastTowerButton.gameObject.SetActive(chloroTowerUnlocked);

        bool mitoTowerUnlocked = Util.upgradeSettings.mitoTowerUnlocked;
        mitoTowerButton.gameObject.SetActive(mitoTowerUnlocked);
    }

    private void OnEnable()
    {
        EventManager.Game.onLevelStart += OnLevelStart;
        EventManager.Game.onSpendCurrency += OnSpendCurrency;
        EventManager.Game.onGainCurrency += OnGainCurrency;
        EventManager.Structures.onStructureCooldownStarted += OnStructureCooldownStarted;
    }

    private void OnDisable()
    {
        EventManager.Game.onLevelStart -= OnLevelStart;
        EventManager.Game.onSpendCurrency -= OnSpendCurrency;
        EventManager.Game.onGainCurrency -= OnGainCurrency;
        EventManager.Structures.onStructureCooldownStarted -= OnStructureCooldownStarted;
    }
}
}
