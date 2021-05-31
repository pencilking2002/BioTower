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
            TapCrystal(hitInfo);
            TapLightFragment(hitInfo);
        }
    }

    private void TapCrystal(RaycastHit2D hitInfo)
    {
        EnemyCrystal crystal = hitInfo.collider.transform.parent.GetComponent<EnemyCrystal>();
        if (crystal != null)
        {
            crystal.DestroyObject();
            GameManager.Instance.econManager.GainCrystalMoney();
        }
    }

    private void TapLightFragment(RaycastHit2D hitInfo)
    {
        LightFragment fragment = hitInfo.collider.transform.parent.GetComponent<LightFragment>();
        if (fragment != null)
        {
            fragment.DestroyObject();
            GameManager.Instance.econManager.GainCrystalMoney();
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