using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using Sirenix.OdinInspector;
using UnityEngine.UI;

namespace BioTower
{
    public class TutorialCanvas : MonoBehaviour
    {
        public static bool tutorialInProgress;


        [Header("Tut UI")]
        public Transform tutPanel;
        public Transform itemHighlightPanel;
        public PortraitController portraitController;
        public TextMeshProUGUI tutText;
        public CanvasGroup ctaText;
        public Button skipButton;


        [Header("Tut State")]
        public bool hasTutorials;
        public TutState tutState;
        public bool skipTutorials;
        [ShowIf("hasTutorials")] public int currTutorialIndex = -1;
        [ShowIf("hasTutorials")] public TutorialData[] tutorials;


        [Header("Tut Animation")]
        public int slideInOffset = 50;
        public float revealDuration = 0.05f;

        public TutorialData currTutorial => tutorials[currTutorialIndex];
        [HideInInspector] public Canvas canvas;
        [HideInInspector] public Vector3 initTutPanelLocalPos;
        [HideInInspector] public WordAnimation tutTextWordAnim;
        private Dictionary<TutState, TutStateBase> charStates = new Dictionary<TutState, TutStateBase>();

        private void Awake()
        {
            Util.tutCanvas = this;

            if (!hasTutorials)
            {
                gameObject.SetActive(false);
                return;
            }
            canvas = GetComponent<Canvas>();
            tutTextWordAnim = tutText.GetComponent<WordAnimation>();
            skipButton.gameObject.SetActive(false);
            CacheStates();
        }

        public bool IsLastTutorial(TutorialData data)
        {
            int index = Array.IndexOf(tutorials, data);
            return index >= tutorials.Length - 1;
        }

        private void CacheStates()
        {
            var states = GetComponentsInChildren<TutStateBase>();
            foreach (TutStateBase state in states)
                charStates.Add(state.tutState, state);
        }

        public void HideSkipButton()
        {
            var cg = skipButton.GetComponent<CanvasGroup>();
            LeanTween.alphaCanvas(cg, 0, 1.0f)
            .setOnComplete(() =>
            {
                skipButton.gameObject.SetActive(false);
            });
        }

        public void OnPressSkipTutButton()
        {
            if (!IsEndTutState())
            {
                SetEndTutState();
                skipTutorials = true;
                HideSkipButton();
                EventManager.Tutorials.onSkipTutorials?.Invoke();
            }
        }

        private void Update()
        {
            if (!GameManager.Instance.gameStates.IsGameState())
                return;

            tutState = charStates[tutState].OnUpdate(tutState);
        }


        public void SetNoneState() { tutState = TutState.NONE; }
        public void SetLetterRevealState() { tutState = TutState.LETTER_REVEAL; }
        public void SetWaitingTapState() { tutState = TutState.WAITING_TAP; }
        public void SetWaitingButtonTapState() { tutState = TutState.WAITING_BUTTON_TAP; }
        public void SetEndTutState() { tutState = TutState.END; }

        public bool IsNoneState() { return tutState == TutState.NONE; }
        public bool IsLetterRevealState() { return tutState == TutState.LETTER_REVEAL; }
        public bool IsWaitingTapState() { return tutState == TutState.WAITING_TAP; }
        public bool IsWaitingButtonTapState() { return tutState == TutState.WAITING_BUTTON_TAP; }
        public bool IsEndTutState() { return tutState == TutState.END; }
    }
}
