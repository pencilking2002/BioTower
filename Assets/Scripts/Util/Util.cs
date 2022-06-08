using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;
using BioTower.SaveData;
using BioTower.Structures;
using UnityEngine.SceneManagement;
using BioTower.Units;

namespace BioTower
{
    public class Util : MonoBehaviour
    {
        public static GameSettings gameSettings => GameManager.Instance.gameSettings;
        public static Params upgradeSettings => GameManager.Instance.gameSettings.upgradeSettings;
        public static UpgradeTree upgradeTree => GameManager.Instance.upgradeTree;
        public static CrystalManager crystalManager => GameManager.Instance.crystalManager;
        public static EconomyManager econManager => GameManager.Instance.econManager;
        public static SaveSystem saveManager => GameManager.Instance.saveManager;
        public static BootController bootController => GameManager.Instance.bootController;
        public static PoolManager poolManager => GameManager.Instance.poolManager;
        public static PlacementManager placementManager => GameManager.Instance.placementManager;
        public static StructureManager towerManager => GameManager.Instance.structureManager;
        public static TapManager tapManager => GameManager.Instance.tapManager;
        public static GameStates gameStates => GameManager.Instance.gameStates;
        public static WaveManager waveManager => GameManager.Instance.waveManager;
        public static ColorManager colorManager => GameManager.Instance.colorManager;
        public static StructureManager structureManager => GameManager.Instance.structureManager;
        public static CooldownManager cooldownManager => GameManager.Instance.cooldownManager;
        public static BombPanel bombPanel => GameManager.Instance.bootController.bombPanel;
        public static UnitManager unitManager => GameManager.Instance.unitManager;
        public static TutorialCanvas tutCanvas;
        public static ObjectShake objectShake => GameManager.Instance.objectShake;
        public Color hurtColor;

        // Layers -----------------------------------------------
        public static int structureSocketLayer = 12;
        public static int enemyLayer = 10;
        public LayerMask obstaclesLayerMask;
        public LayerMask enemyLayerMask;
        public static int uiLayer = 5;

        public static void ScaleBounceSprite(SpriteRenderer sr, float scaleUpFactor)
        {
            var oldScale = sr.transform.localScale;
            LeanTween.scale(sr.gameObject, oldScale * scaleUpFactor, 0.1f).setOnComplete(() =>
            {
                LeanTween.scale(sr.gameObject, oldScale, 0.1f);
            });
        }

        public static void ScaleUpSprite(SpriteRenderer sr, float scaleUpFactor)
        {
            var oldScale = sr.transform.localScale;
            sr.transform.localScale = Vector3.zero;
            LeanTween.scale(sr.gameObject, oldScale * scaleUpFactor, 0.1f).setOnComplete(() =>
            {
                LeanTween.scale(sr.gameObject, oldScale, 0.1f);
            });
        }

        // public static void AnimateBounceSpriteColor(SpriteRenderer sr, Color color)
        // {
        //     var oldColor = sr.color;
        //     LeanTween.value(sr.gameObject, oldColor, Color.red, 0.1f).setOnComplete(() => {
        //         sr.color = 
        //     });
        // }

        public static Vector2 Bezier2(Vector2 start, Vector2 control, Vector2 end, float t)
        {
            return (((1 - t) * (1 - t)) * start) + (2 * t * (1 - t) * control) + ((t * t) * end);
        }

        public static Vector2 GetPointWithinInfluence(Vector2 centerPoint, float minRadius, float maxRadius)
        {
            Vector2 point = UnityEngine.Random.insideUnitCircle.normalized * UnityEngine.Random.Range(minRadius, maxRadius);
            point += centerPoint;
            return point;
        }

        public enum ButtonColorMode
        {
            DEFAULT,
            SELECTED
        }

        public static void HandleInvalidButtonPress(RectTransform button, ButtonColorMode colorMode = ButtonColorMode.SELECTED)
        {
            if (colorMode == ButtonColorMode.SELECTED)
            {
                var btn = button.GetComponentInChildren<Button>();
                var colors = btn.colors;
                Color oldColor = colors.selectedColor;
                colors.selectedColor = Color.red;
                btn.colors = colors;
                LeanTween.delayedCall(0.15f, () =>
                {
                    colors.selectedColor = oldColor;
                    btn.colors = colors;
                });
            }
            else
            {
                var image = button.GetComponent<Image>();
                Color oldColor = image.color;
                image.color = Color.red;
                LeanTween.delayedCall(0.15f, () =>
                {
                    image.color = oldColor;
                });
            }
            EventManager.UI.onTapButton?.Invoke(false);
        }

        public void TextReveal(TMP_Text text, float revealDuration, Action onComplete = null)
        {
            if (text == null)
                return;

            StartCoroutine(RevealCharacters(text, revealDuration, onComplete));
            EventManager.Tutorials.onTutChatStart?.Invoke();
        }

        public IEnumerator RevealCharacters(TMP_Text textComponent, float revealDuration, Action onComplete = null)
        {
            textComponent.ForceMeshUpdate();
            int visibleCount = 0;
            TMP_TextInfo textInfo = textComponent.textInfo;
            int totalVisibleCharacters = textInfo.characterCount; // Get # of Visible Character in text object

            if (LetterRevealState.cancelLetterReveal)
            {
                visibleCount = totalVisibleCharacters;
                textComponent.maxVisibleCharacters = visibleCount;
                onComplete?.Invoke();
                yield return null;
            }

            while (visibleCount <= totalVisibleCharacters)
            {
                // Exit out of the letter revealing early
                if (LetterRevealState.cancelLetterReveal)
                {
                    textComponent.ForceMeshUpdate();
                    visibleCount = totalVisibleCharacters;
                    textComponent.maxVisibleCharacters = visibleCount;
                    onComplete?.Invoke();
                    yield return null;
                }

                if (!LetterRevealState.cancelLetterReveal)
                {
                    textComponent.maxVisibleCharacters = visibleCount; // How many characters should TextMeshPro display?
                    visibleCount += 1;
                    EventManager.UI.onLetterReveal?.Invoke();
                    yield return new WaitForSeconds(revealDuration);
                }
            }

            if (visibleCount >= totalVisibleCharacters && !LetterRevealState.cancelLetterReveal)
            {
                onComplete?.Invoke();
                yield return null;
            }
        }

        public static void DisplayGlowUI(Transform uiElement)
        {
            uiElement.parent.Find("Glow").GetComponent<Image>().enabled = true;
        }

        public static void HideGlowUI(Transform uiElement)
        {
            uiElement.parent.Find("Glow").GetComponent<Image>().enabled = false;
        }

        public static Vector3 GetUIWorldPos(RectTransform rt)
        {
            return Camera.main.ScreenToWorldPoint(rt.position);
        }

        public static void ReloadLevel()
        {
            BootController.isBootLoaded = false;
            GameManager.Instance.LoadLevel(Util.upgradeSettings.currLevel);
        }
    }
}