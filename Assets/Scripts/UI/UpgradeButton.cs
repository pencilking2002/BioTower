using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace BioTower
{
    public class UpgradeButton : MonoBehaviour
    {
        [SerializeField] private UpgradeType upgradeType = UpgradeType.NONE;
        [SerializeField] private Image icon;
        public Image image;
        [SerializeField] private TextMeshProUGUI text;

        private void Awake()
        {
            image = GetComponent<Image>();
        }

        public void SetIcon(Sprite sprite)
        {
            icon.sprite = sprite;
        }

        public void SetText(string str)
        {
            text.text = str;
        }

        public void SetUpgradeType(UpgradeType upgradeType)
        {
            this.upgradeType = upgradeType;
        }

        public UpgradeType GetUpgradeType()
        {
            return upgradeType;
        }
    }
}