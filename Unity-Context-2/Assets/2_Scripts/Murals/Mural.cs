using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Mural : IFixedUpdateable
{
    public event Action OnSequenceDone;
    public bool LibraryMural { get; private set; }

    private GameObject gameObject;

    // Mushrooms
    private List<Animator> mushroomAnimators = new List<Animator>();
    private List<BubbleController> mushroomBubbles = new List<BubbleController>();
    private List<LoopingSound> mushroomSounds = new List<LoopingSound>();
    private List<MuralMushroomPrefab> mushroomPrefabs;
    private float mushroomDistance;

    // Chimes
    public ChimeSequence ChimeSequence { get; private set; }
    private float detectRange;
    private int playingIndex;

    // Times
    private Timer chimeTimer;
    private Timer repeatTimer;
    private float shortChimeTime;
    private float longChimeTime;

    // References
    private PlayerController player;
    private SoundManager soundManager;
    private TimerManager timerManager;

    //------------------------------------

    public Mural(ChimeSequence chimeSequence, GameObject gameObject, bool libraryMural){
        this.ChimeSequence = chimeSequence;
        this.gameObject = gameObject;
        LibraryMural = libraryMural;
        player = GameManager.Instance.Player;
        detectRange = MuralSettings.Instance.DetectRange;
        mushroomPrefabs = MuralSettings.Instance.MushroomPrefabs;
        mushroomDistance = MuralSettings.Instance.MushroomDistance;
        shortChimeTime = MuralSettings.Instance.ShortChimePlayDelay;
        longChimeTime = MuralSettings.Instance.LongChimePlayDelay;
        soundManager = GameManager.GetService<SoundManager>();
        timerManager = GameManager.GetService<TimerManager>();

        chimeTimer = timerManager.AddLoopingTimer(shortChimeTime);
        repeatTimer = timerManager.AddTimer(MuralSettings.Instance.RepeatDelay);
        repeatTimer.ChangeCurrentTime(0);

        if (!libraryMural) SpawnMushrooms();

        #if UNITY_EDITOR
            if (detectRange == 0) { Debug.LogWarning("DetectRange in MuralSettings is 0"); }
        #endif
    }

    public void OnFixedUpdate(){

        if (playingIndex == 0){
            if (Vector3.Distance(gameObject.transform.position, player.gameObject.transform.position) > detectRange) { return; }
        }
        if (!repeatTimer.IsDone()) { return; }
        if (!chimeTimer.IsDone()) { return; }

        // If sequence is done
        if (playingIndex == ChimeSequence.chimes.Count + 1){
            playingIndex = 0;
            repeatTimer.Reset();
            if (OnSequenceDone != null) { OnSequenceDone(); }
            return;
        }

        // Chimes
        PlayChime();
        StopChime();

        // Set timer lenght
        if (playingIndex < ChimeSequence.chimes.Count){
            if (ChimeSequence.chimes[playingIndex].isLong){
                chimeTimer.ChangeLenght(longChimeTime);
            }
            else{
                chimeTimer.ChangeLenght(shortChimeTime);
            }
        }

        chimeTimer.Reset();
        playingIndex++;
    }

    public void SpawnMushrooms(){
        Vector3 dir = gameObject.transform.right;
        Vector3 centerPoint = gameObject.transform.position;
        float totalLength = mushroomDistance * (ChimeSequence.chimes.Count - 1);
        Vector3 startPosition = centerPoint - dir * totalLength / 2f;
            
        for (int i = 0; i < ChimeSequence.chimes.Count; i++){
            ChimeInputs chimeInput = ChimeSequence.chimes[i].chimeInput;
            Vector3 mushroomPos = startPosition + dir * mushroomDistance * i;
            GameObject newMushroom = GameObject.Instantiate(mushroomPrefabs.Find(chimePrefab => chimePrefab.chimeInputs == chimeInput).gameObject, mushroomPos, Quaternion.identity, gameObject.transform);
            if (ChimeSequence.chimes[i].isLong) { newMushroom.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f); }
            Animator mushroomAnimator = newMushroom.GetComponent<Animator>();
            BubbleController mushroomBubble = newMushroom.GetComponentInChildren<BubbleController>();

            #if UNITY_EDITOR
                if (mushroomAnimator == null) { Debug.LogError("Mushroom doesn't contain a animator"); }
                if (mushroomBubble == null) { Debug.LogError("Mushroom doesn't contain a VisualEffect"); }
            #endif

            mushroomAnimators.Add(mushroomAnimator);
            mushroomBubbles.Add(mushroomBubble);
            mushroomSounds.Add(default);
        }
    }

    //----------------------------------------

    private void PlayChime(){
        if (playingIndex >= ChimeSequence.chimes.Count) { return; }

        mushroomAnimators[playingIndex].SetBool("Active", true);
        mushroomBubbles[playingIndex].StartBubble(ChimeSequence.chimes[playingIndex].chimeInput);
        PlayerSoundData playerSoundData = soundManager.GetPlayerSound(ChimeSequence.chimes[playingIndex].chimeInput);
        mushroomSounds[playingIndex] = soundManager.PlayLoopingSound(playerSoundData, gameObject.transform.position);
    }

    private void StopChime(){
        if (playingIndex == 0) { return; }

        mushroomSounds[playingIndex - 1].StopSound();
        mushroomAnimators[playingIndex - 1].SetBool("Active", false);
        mushroomBubbles[playingIndex - 1].StopBubble();
    }
}
