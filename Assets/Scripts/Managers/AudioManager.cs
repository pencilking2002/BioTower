using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BioTower
{
public class AudioManager : MonoBehaviour
{
    public AudioData data;
    public AudioSource musicSource;
    public AudioSource sfxSource;

    private void PlaySound(AudioClip clip) { sfxSource.PlayOneShot(clip); }

    private void PlayMusic(AudioClip clip) 
    { 
        musicSource.clip = clip;
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
                PlayMusic(data.gameOverLose);
                break;
            case GameState.GAME_OVER_WIN:
                PlayMusic(data.gameOverWin);
                break;
        }
    }

    private void OnBaseTakeDamage()
    {
        PlaySound(data.enemyBaseAttacked);
    }

    private void OnTutorialStart(TutorialData tut)
    {
        PlaySound(data.textPopUp);
    }

    private void OnEnable()
    {
        EventManager.Game.onGameStateInit += OnGameStateInit;
        EventManager.Structures.onBaseTakeDamage += OnBaseTakeDamage;
        EventManager.Tutorials.onTutorialStart += OnTutorialStart;
    }

    private void OnDisable()
    {
        EventManager.Game.onGameStateInit -= OnGameStateInit;
        EventManager.Structures.onBaseTakeDamage -= OnBaseTakeDamage;
        EventManager.Tutorials.onTutorialStart -= OnTutorialStart;
    }
}
}
