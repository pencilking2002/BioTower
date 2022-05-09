using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BioTower
{
    public class BombPanel : MonoBehaviour
    {
        public GameObject bombPrefab;
        [SerializeField] private Image progressImage;
        [SerializeField] private Button bombButton;

        public void OnPressBombButton()
        {
            if (Util.placementManager.IsNoneState())
                Util.placementManager.SetBombPlacingState();
        }
    }
}
