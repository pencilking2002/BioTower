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

        private void PlayMusic(AudioClip clip, bool loop = true)
        {
            musicSource.clip = clip;
            musicSource.loop = loop;
            musicSource.Play();
        }

        private void PlayMusicCrossFade(AudioClip clip, float crossFadeDuration, bool loop = true)
        {
            if (musicSource.clip != null)
            {
                Debug.Log("Play");

                var currVol = musicSource.volume;
                var seq = LeanTween.sequence();

                seq.append(LeanTween.value(gameObject, currVol, 0, crossFadeDuration)
                    .setOnUpdate((float val) => { musicSource.volume = val; }));

                seq.append(gameObject, () =>
                {
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

        // GAME ----------------------------------------------

        private void OnLevelStart(LevelType levelType)
        {
            if (LevelInfo.current.HasTutorials())
                PlayMusicCrossFade(data.goofyPlantsTrack, 0.5f);
            else
                PlayMusicCrossFade(data.levelTrack_01, 0.5f);
        }

        private void OnGameStateInit(GameState gameState)
        {
            switch (gameState)
            {
                case GameState.GAME_OVER_LOSE:
                    PlayMusic(data.gameOverLose, true);
                    break;
                case GameState.GAME_OVER_WIN:
                    PlayMusic(data.gameOverWin, false);
                    break;
                case GameState.START_MENU:
                    PlayMusicCrossFade(data.mainMenuTrack, 0.5f);
                    break;
                case GameState.LEVEL_SELECT:
                    Debug.Log("level select");
                    if (musicSource.isPlaying)
                    {
                        if (musicSource.clip != data.mainMenuTrack)
                            PlayMusicCrossFade(data.mainMenuTrack, 0.5f);
                    }
                    else
                        PlayMusic(data.mainMenuTrack);

                    break;
            }
        }

        private void OnSpendCurrency(int num, int playerCurrency)
        {
            if (playerCurrency == 0)
                PlaySound(data.energyGone);
        }

        private void OnSnrk2UnitReachedBase(Snrk2Unit unit)
        {
            PlaySound(data.crystalDeposited);
        }

        // WAVES ---------------------------------------------

        private void OnWaveStateInit(WaveMode waveMode)
        {
            if (waveMode == WaveMode.IN_PROGRESS)
                PlaySound(data.waveStarted);
            else if (waveMode == WaveMode.ENDED)
                PlaySound(data.waveDefeated);
        }

        private void OnWaveCountdownTick(int num)
        {
            if (num <= 3)
                PlaySound(data.tick);
        }

        // TUTORIAL ---------------------------------------------

        private void OnTutTextPopUp()
        {
            PlaySound(data.textPopUp);
        }

        private void OnTutChatStart()
        {
            PlaySound(data.tutChat);
        }

        private void OnTutorialEnd(TutorialData tutorialData)
        {
            PlayMusicCrossFade(data.levelTrack_01, 0.5f);
        }

        // UNITS ---------------------------------------------

        private void OnUnitSpawned(Unit unit)
        {
            if (unit.unitType == UnitType.ABA || unit.unitType == UnitType.SNRK2)
            {
                PlaySound(data.abaSpawned);
            }
        }

        private void OnCrystalPickedUp(Snrk2Unit unit)
        {
            PlaySound(data.crystalPickedUp);
        }

        private void OnEnemyPickupCrystal()
        {
            PlaySound(data.enemyCrystalPickedUp);
        }

        private void OnUnitTakeDamage(UnitType unitType)
        {
            var randIndex = UnityEngine.Random.Range(0, data.takeDamage.Length - 1);
            PlaySound(data.takeDamage[randIndex]);
        }

        private void OnUnitDestroyed(Unit unit)
        {
            PlaySound(data.explode);
        }

        // STRUCTURES ---------------------------------------------

        private void OnBaseTakeDamage()
        {
            PlaySound(data.enemyBaseAttacked);
        }

        private void OnStructureCreated(Structure tower, bool doSquishyAnim)
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

        private void OnSocketPop(StructureSocket socket)
        {
            PlaySound(data.structureSocketPopUp);
        }

        private void OnTapFreeStructureSocket(StructureSocket socket)
        {
            var selectedButtonType = Util.bootController.gameplayUI.GetSelectedButtonType();

            if (!Util.placementManager.IsPlacingState() && selectedButtonType == StructureType.NONE)
            {
                PlaySound(data.structureSocketTap);
            }
        }

        // UI ---------------------------------------------

        private void OnTapButton(bool isValid)
        {
            var clip = isValid ? data.btnSelected : data.invalidBtnSelected;
            PlaySound(clip);
        }

        private void OnPressLevelSelectButton()
        {
            PlaySound(data.levelSelect);
        }

        private void OnTitleAnimCompleted(int numTimes)
        {
            if (Util.gameStates.IsGameState())
                return;

            if (numTimes == 1)
                PlaySoundWithPitch(data.titleDrop, 1);
            else
                PlaySoundWithPitch(data.titleDrop, 0.7f);
        }


        private void OnEnable()
        {
            EventManager.Game.onLevelStart += OnLevelStart;
            EventManager.Game.onGameStateInit += OnGameStateInit;
            EventManager.Game.onSpendCurrency += OnSpendCurrency;
            EventManager.Game.onSnrk2UnitReachedBase += OnSnrk2UnitReachedBase;

            EventManager.Wave.onWaveStateInit += OnWaveStateInit;
            EventManager.Wave.onWaveCountdownTick += OnWaveCountdownTick;

            EventManager.Tutorials.onTutChatStart += OnTutChatStart;
            EventManager.Tutorials.onTutTextPopUp += OnTutTextPopUp;
            EventManager.Tutorials.onTutorialEnd += OnTutorialEnd;

            EventManager.Units.onUnitSpawned += OnUnitSpawned;
            EventManager.Units.onCrystalPickedUp += OnCrystalPickedUp;
            EventManager.Units.onEnemyPickedUpCrystal += OnEnemyPickupCrystal;
            EventManager.Units.onUnitTakeDamage += OnUnitTakeDamage;
            EventManager.Units.onUnitDestroyed += OnUnitDestroyed;

            EventManager.Structures.onBaseTakeDamage += OnBaseTakeDamage;
            EventManager.Structures.onStructureCreated += OnStructureCreated;
            EventManager.Structures.onStructureGainHealth += OnStructureGainHealth;
            EventManager.Structures.onStructureSelected += OnStructureSelected;
            EventManager.Structures.onStructureDestroyed += OnStructureDestroyed;
            EventManager.Structures.onLightPickedUp += OnLightPickedUp;
            EventManager.Structures.onLightDropped += OnLightDropped;
            EventManager.Structures.onSocketPop += OnSocketPop;
            EventManager.Structures.onTapFreeStructureSocket += OnTapFreeStructureSocket;

            EventManager.UI.onTapButton += OnTapButton;
            EventManager.UI.onPressLevelSelectButton += OnPressLevelSelectButton;
            EventManager.UI.onTitleAnimCompleted += OnTitleAnimCompleted;
        }

        private void OnDisable()
        {
            EventManager.Game.onLevelStart -= OnLevelStart;
            EventManager.Game.onGameStateInit -= OnGameStateInit;
            EventManager.Game.onSpendCurrency -= OnSpendCurrency;
            EventManager.Game.onSnrk2UnitReachedBase -= OnSnrk2UnitReachedBase;

            EventManager.Wave.onWaveStateInit -= OnWaveStateInit;
            EventManager.Wave.onWaveCountdownTick -= OnWaveCountdownTick;

            EventManager.Tutorials.onTutChatStart -= OnTutChatStart;
            EventManager.Tutorials.onTutTextPopUp -= OnTutTextPopUp;
            EventManager.Tutorials.onTutorialEnd -= OnTutorialEnd;

            EventManager.Units.onUnitSpawned -= OnUnitSpawned;
            EventManager.Units.onCrystalPickedUp -= OnCrystalPickedUp;
            EventManager.Units.onEnemyPickedUpCrystal -= OnEnemyPickupCrystal;
            EventManager.Units.onUnitTakeDamage -= OnUnitTakeDamage;
            EventManager.Units.onUnitDestroyed -= OnUnitDestroyed;

            EventManager.Structures.onBaseTakeDamage -= OnBaseTakeDamage;
            EventManager.Structures.onStructureCreated -= OnStructureCreated;
            EventManager.Structures.onStructureGainHealth -= OnStructureGainHealth;
            EventManager.Structures.onStructureSelected -= OnStructureSelected;
            EventManager.Structures.onStructureDestroyed -= OnStructureDestroyed;
            EventManager.Structures.onLightPickedUp -= OnLightPickedUp;
            EventManager.Structures.onLightDropped -= OnLightDropped;
            EventManager.Structures.onSocketPop -= OnSocketPop;
            EventManager.Structures.onTapFreeStructureSocket -= OnTapFreeStructureSocket;

            EventManager.UI.onTapButton -= OnTapButton;
            EventManager.UI.onPressLevelSelectButton -= OnPressLevelSelectButton;
            EventManager.UI.onTitleAnimCompleted -= OnTitleAnimCompleted;
        }
    }
}
