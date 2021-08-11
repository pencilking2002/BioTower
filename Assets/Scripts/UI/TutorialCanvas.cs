using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;

namespace BioTower
{
public class TutorialCanvas : MonoBehaviour
{

    [Header("Tutorial Data")]
    [SerializeField] private TutorialData[] tutorials;
    [SerializeField] private int currTutorial = -1;
    [SerializeField] private bool initTutorialOnStart;


    [Header("Tutorial UI")]
    public Image tutPanel;
    public TextMeshProUGUI tutText;
    [SerializeField] private int slideInOffset = 50;
    private Vector3 initTutPanelLocalPos;

    private void Start()
    {
        initTutPanelLocalPos = tutPanel.transform.localPosition;

        if (initTutorialOnStart)
            StartNextTutorial();
    }

    private void SetupPanel()
    {
       
    }

    public void StartNextTutorial()
    {
        currTutorial++;
        var currTut = tutorials[currTutorial];
        tutText.text = currTut.text;

        if (currTut.transition == TransitionType.SLIDE_IN)
        {
            tutPanel.transform.localPosition += new Vector3(0, slideInOffset, 0);
            var seq = LeanTween.sequence();
            seq.append(currTut.delay);
            seq.append(LeanTween.moveLocalY(tutPanel.gameObject, initTutPanelLocalPos.y, 0.25f));
        }
       
        EventManager.Tutorials.onTutorialStart?.Invoke(tutorials[currTutorial]);
    }

}
}
