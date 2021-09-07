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

    private void PlaySound(AudioClip clip) { sfxSource.PlayOneShot(clip); }

    private void PlayMusic(AudioClip clip, bool loop=true) 
    { 
        musicSource.clip = clip;
        musicSource.loop = loop;
        musicSource.Play(); 
    }

    private void PlaySoundAtLocation(AudioClip clip, Vector3 location, float vol)
    {
        AudioSource.PlayClipAtPoint(clip, location, vol);
    }

    private void OnGameStateInit(GameState gameState)
    {
        switch(gameState)
        {
            case GameState.GAME:
                PlayMusic(data.levelTrack);
                break;
            case GameState.GAME_OVER_LOSE:
                PlayMusic(data.gameOverLose, false);
                break;
            case GameState.GAME_OVER_WIN:
                PlayMusic(data.gameOverWin, false);
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
        if (unit.unitType == UnitType.ABA)
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

    private void OnTapButton()
    {
        PlaySound(data.btnSelected);
    }

    private void OnLetterRevealed()
    {
        PlaySound(data.letterRevealed);
    }

    private void OnEnable()
    {
        EventManager.Game.onGameStateInit += OnGameStateInit;
        EventManager.Structures.onBaseTakeDamage += OnBaseTakeDamage;
        EventManager.Tutorials.onTutTextPopUp += OnTutTextPopUp;
        EventManager.Units.onUnitSpawned += OnUnitSpawned;
        EventManager.Game.onSnrk2UnitReachedBase += OnSnrk2UnitReachedBase;
        EventManager.Units.onCrystalPickedUp += OnCrystalPickedUp;
        EventManager.Structures.onStructureCreated += OnStructureCreated;
        EventManager.Structures.onStructureGainHealth += OnStructureGainHealth;
        EventManager.Structures.onStructureSelected += OnStructureSelected;
        EventManager.Structures.onStructureDestroyed += OnStructureDestroyed;
        EventManager.Structures.onLightDropped += OnLightDropped;
        EventManager.Structures.onLightPickedUp += OnLightPickedUp;
        EventManager.UI.onTapButton += OnTapButton;
        EventManager.UI.onLetterReveal += OnLetterRevealed;
    }

    private void OnDisable()
    {
        EventManager.Game.onGameStateInit -= OnGameStateInit;
        EventManager.Structures.onBaseTakeDamage -= OnBaseTakeDamage;
        EventManager.Tutorials.onTutTextPopUp -= OnTutTextPopUp;
        EventManager.Units.onUnitSpawned -= OnUnitSpawned;
        EventManager.Game.onSnrk2UnitReachedBase -= OnSnrk2UnitReachedBase;
        EventManager.Units.onCrystalPickedUp -= OnCrystalPickedUp;
        EventManager.Structures.onStructureCreated -= OnStructureCreated;
        EventManager.Structures.onStructureGainHealth -= OnStructureGainHealth;
        EventManager.Structures.onStructureSelected -= OnStructureSelected;
        EventManager.Structures.onStructureDestroyed -= OnStructureDestroyed;
        EventManager.Structures.onLightDropped -= OnLightDropped;
        EventManager.UI.onTapButton -= OnTapButton;
        EventManager.UI.onLetterReveal -= OnLetterRevealed;
    }
}
}
