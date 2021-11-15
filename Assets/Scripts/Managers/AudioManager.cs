using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BioTower.Units;
using BioTower.Structures;
using BioTower.UI;

namespace BioTower
{
public class AudioManager : MonoBehaviour
{
    public AudioData data;
    public AudioSource musicSource;
    public AudioSource sfxSource;

    private void PlaySound(AudioClip clip) 
    {
        if (clip != null) 
        {
            if (sfxSource == null)
            {
                sfxSource = this.transform.Find("sfxSource").GetComponent<AudioSource>();
            }
            sfxSource.PlayOneShot(clip); 
        }
    }

    private void PlayMusic(AudioClip clip, bool loop=true) 
    { 
        musicSource.clip = clip;
        musicSource.loop = loop;
        musicSource.Play(); 
    }

    private void PlayMusicCrossFade(AudioClip clip, float crossFadeDuration, bool loop=true) 
    { 
        if (musicSource.clip != null)
        {
            var currVol = musicSource.volume;
            var seq = LeanTween.sequence();
        
            seq.append(LeanTween.value(gameObject, currVol, 0, crossFadeDuration)
                .setOnUpdate((float val) => { musicSource.volume = val; }));

            seq.append(gameObject, () => {
                musicSource.clip = clip;
                musicSource.loop = loop;
                musicSource.Play(); 
            });

            seq.append(LeanTween.value(gameObject, 0, currVol, crossFadeDuration)
                .setOnUpdate((float val) => { musicSource.volume = val; }));   
        }
        else
        {
            PlayMusic(clip, loop);
        }
    }

    private void PlaySoundAtLocation(AudioClip clip, Vector3 location, float vol)
    {
        AudioSource.PlayClipAtPoint(clip, location, vol);
    }

    private void PlaySoundWithPitch(AudioClip clip, float pitch)
    {
        var go = new GameObject();
        var src = go.AddComponent<AudioSource>();
        src.loop = false;
        src.clip = clip;
        src.pitch = pitch;
        src.Play();
        Destroy(go, clip.length);
    }

    private void OnGameStateInit(GameState gameState)
    {
        switch(gameState)
        {
            // case GameState.GAME:
            //     if (!LevelInfo.current.HasTutorials())
            //         PlayMusicCrossFade(data.levelTrack_01, 0.5f);
            //     break;
            case GameState.GAME_OVER_LOSE:
                PlayMusic(data.gameOverLose, true);
                break;
            case GameState.GAME_OVER_WIN:
                PlayMusic(data.gameOverWin, false);
                break;
            case GameState.START_MENU:
                PlayMusicCrossFade(data.mainMenuTrack, 0.5f);
                break;
        }
    }

    private void OnBaseTakeDamage()
    {
        PlaySound(data.enemyBaseAttacked);
    }

    private void OnTutTextPopUp()
    {
        PlaySound(data.textPopUp);
    }

    private void OnUnitSpawned(Unit unit)
    {
        if (unit.unitType == UnitType.ABA || unit.unitType == UnitType.SNRK2)
        {
            PlaySound(data.abaSpawned);
        }
    }

    private void OnSnrk2UnitReachedBase(Snrk2Unit unit)
    {
        PlaySound(data.crystalDeposited);
    }

    private void OnCrystalPickedUp(Snrk2Unit unit)
    {
        PlaySound(data.crystalPickedUp);
    }

    private void OnEnemyPickupCrystal()
    {
        PlaySound(data.enemyCrystalPickedUp);
    }

    private void OnStructureCreated(Structure tower)
    {
        if (tower.structureType != StructureType.DNA_BASE)
            PlaySound(data.towerPlaced);
    }

    private void OnStructureGainHealth(Structure tower)
    {
        PlaySound(data.towerHealed);
    }
    private void OnStructureSelected(Structure tower)
    {
        PlaySound(data.towerSelected);
    }

    private void OnStructureDestroyed(Structure tower)
    {
        PlaySound(data.towerDeath);
    }

    private void OnLightDropped()
    {
        PlaySound(data.lightDropped);
    }

    private void OnLightPickedUp()
    {
        PlaySound(data.lightPickedUp);
    }

    private void OnTapButton(bool isValid)
    {
        var clip = isValid ? data.btnSelected : data.invalidBtnSelected;
        PlaySound(clip);
    }

    private void OnPressLevelSelectButton()
    {
        PlaySound(data.levelSelect);
    }

    private void OnUnitTakeDamage(UnitType unitType)
    {
        var randIndex = UnityEngine.Random.Range(0, data.takeDamage.Length-1);
        //Debug.Log($"randIndex: {randIndex}. arr length: {data.takeDamage.Length}");
        PlaySound(data.takeDamage[randIndex]);
    }

    private void OnTutChatStart()
    {
        PlaySound(data.tutChat);
    }

    private void OnTitleAnimCompleted(int numTimes)
    {
        if (numTimes == 1)
            PlaySoundWithPitch(data.titleDrop, 1);
        else
            PlaySoundWithPitch(data.titleDrop, 0.7f);
    }

    private void OnUnitDestroyed(Unit unit)
    {
        PlaySound(data.explode);
    }

    private void OnSpendCurrency(int num, int playerCurrency)
    {
        if (playerCurrency == 0)
            PlaySound(data.energyGone);
    }

    private void OnLevelStart(LevelType levelType)
    {
        if (LevelInfo.current.HasTutorials())
            PlayMusicCrossFade(data.goofyPlantsTrack, 0.5f);
        else
            PlayMusicCrossFade(data.levelTrack_01, 0.5f);
    }

    private void OnTutorialEnd(TutorialData tutorialData)
    {
        PlayMusicCrossFade(data.levelTrack_01, 0.5f);
    }

    private void OnEnable()
    {
        EventManager.Game.onGameStateInit += OnGameStateInit;
        EventManager.Structures.onBaseTakeDamage += OnBaseTakeDamage;
        EventManager.Tutorials.onTutTextPopUp += OnTutTextPopUp;
        EventManager.Units.onUnitSpawned += OnUnitSpawned;
        EventManager.Game.onSnrk2UnitReachedBase += OnSnrk2UnitReachedBase;
        EventManager.Units.onCrystalPickedUp += OnCrystalPickedUp;
        EventManager.Units.onEnemyPickedUpCrystal += OnEnemyPickupCrystal;
        EventManager.Units.onUnitTakeDamage += OnUnitTakeDamage;
        EventManager.Structures.onStructureCreated += OnStructureCreated;
        EventManager.Structures.onStructureGainHealth += OnStructureGainHealth;
        EventManager.Structures.onStructureSelected += OnStructureSelected;
        EventManager.Structures.onStructureDestroyed += OnStructureDestroyed;
        EventManager.Structures.onLightDropped += OnLightDropped;
        EventManager.Structures.onLightPickedUp += OnLightPickedUp;
        EventManager.UI.onTapButton += OnTapButton;
        EventManager.UI.onPressLevelSelectButton += OnPressLevelSelectButton;
        EventManager.Tutorials.onTutChatStart += OnTutChatStart;
        EventManager.UI.onTitleAnimCompleted += OnTitleAnimCompleted;
        EventManager.Units.onUnitDestroyed += OnUnitDestroyed;
        EventManager.Game.onSpendCurrency += OnSpendCurrency;
        EventManager.Game.onLevelStart += OnLevelStart;
        EventManager.Tutorials.onTutorialEnd += OnTutorialEnd;
    }

    private void OnDisable()
    {
        EventManager.Game.onGameStateInit -= OnGameStateInit;
        EventManager.Structures.onBaseTakeDamage -= OnBaseTakeDamage;
        EventManager.Tutorials.onTutTextPopUp -= OnTutTextPopUp;
        EventManager.Units.onUnitSpawned -= OnUnitSpawned;
        EventManager.Game.onSnrk2UnitReachedBase -= OnSnrk2UnitReachedBase;
        EventManager.Units.onCrystalPickedUp -= OnCrystalPickedUp;
        EventManager.Units.onEnemyPickedUpCrystal -= OnEnemyPickupCrystal;
        EventManager.Units.onUnitTakeDamage -= OnUnitTakeDamage;
        EventManager.Structures.onStructureCreated -= OnStructureCreated;
        EventManager.Structures.onStructureGainHealth -= OnStructureGainHealth;
        EventManager.Structures.onStructureSelected -= OnStructureSelected;
        EventManager.Structures.onStructureDestroyed -= OnStructureDestroyed;
        EventManager.Structures.onLightDropped -= OnLightDropped;
        EventManager.Structures.onLightPickedUp -= OnLightPickedUp;
        EventManager.UI.onTapButton -= OnTapButton;
        EventManager.UI.onPressLevelSelectButton -= OnPressLevelSelectButton;
        EventManager.Tutorials.onTutChatStart -= OnTutChatStart;
        EventManager.UI.onTitleAnimCompleted -= OnTitleAnimCompleted;
        EventManager.Units.onUnitDestroyed -= OnUnitDestroyed;
        EventManager.Game.onSpendCurrency -= OnSpendCurrency;
        EventManager.Game.onLevelStart -= OnLevelStart;
        EventManager.Tutorials.onTutorialEnd -= OnTutorialEnd;
    }
}
}
