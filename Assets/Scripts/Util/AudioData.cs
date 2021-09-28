using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace BioTower
{
[CreateAssetMenu(menuName ="AudioData", fileName="AudioData")]
public class AudioData : ScriptableObject
{
    //[TabGroup("Music")] public AudioClip newWave;
    [TabGroup("Music")] public AudioClip levelTrack;
    [TabGroup("Music")] public AudioClip gameOverLose;
    [TabGroup("Music")] public AudioClip gameOverWin;
    [TabGroup("Music")] public AudioClip mainMenuTrack;

    //[TabGroup("Music")] public AudioClip titleScreenTrack;
   

    //[TabGroup("Units")] public AudioClip unitDeath;
    //[TabGroup("Units")] public AudioClip unitDamage;
    //[TabGroup("Units")] public AudioClip snrk2LeavesTower;
    [TabGroup("Units")] public AudioClip snrk2Spawned;
    [TabGroup("Units")] public AudioClip enemyCrystalPickedUp;
    [TabGroup("Units")] public AudioClip[] takeDamage;
    [TabGroup("Units")] public AudioClip enemyBaseAttacked;
    [TabGroup("Units")] public AudioClip abaSpawned;
    [TabGroup("Units")] public AudioClip crystalDeposited;
    [TabGroup("Units")] public AudioClip crystalPickedUp;
    [TabGroup("Units")] public AudioClip explode;


    [TabGroup("Towers")] public AudioClip towerPlaced;
    [TabGroup("Towers")] public AudioClip towerHealed;
    [TabGroup("Towers")] public AudioClip towerSelected;
    [TabGroup("Towers")] public AudioClip towerDeath;
    [TabGroup("Towers")] public AudioClip lightDropped;
    [TabGroup("Towers")] public AudioClip lightPickedUp;
    //[TabGroup("Towers")] public AudioClip ppc2Shoot;


    [TabGroup("UI")] public AudioClip mitoBtnTapped;    
    [TabGroup("UI")] public AudioClip textPopUp;
    [TabGroup("UI")] public AudioClip btnSelected;
    [TabGroup("UI")] public AudioClip invalidBtnSelected;
    [TabGroup("UI")] public AudioClip letterRevealed;
    [TabGroup("UI")] public AudioClip levelSelect;
    [TabGroup("UI")] public AudioClip tutChat;
    [TabGroup("UI")] public AudioClip titleDrop;
    [TabGroup("UI")] public AudioClip tapAnywhere;
    [TabGroup("UI")] public AudioClip energyGone;

}
}