using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BioTower
{
public class TapManager : MonoBehaviour
{
    [SerializeField] private LayerMask tappableLayerMask;
    private void OnTouchBegan(Vector3 screenPos)
    {
        Ray ray = Camera.main.ScreenPointToRay(screenPos);
        RaycastHit2D hitInfo = Physics2D.Raycast(ray.origin, Vector2.zero, Mathf.Infinity, tappableLayerMask);
        if (hitInfo.collider != null)
        {
            EnemyCrystal crystal = hitInfo.collider.transform.parent.GetComponent<EnemyCrystal>();
            if (crystal != null)
            {
                crystal.DestroyCrystal();
                GameManager.Instance.econManager.GainCrystalMoney();
            }
        }
    }

    private void OnEnable()
    {

        EventManager.Input.onTouchBegan += OnTouchBegan;

    }

    private void OnDisable()
    {

        EventManager.Input.onTouchBegan -= OnTouchBegan;
    }
}
}