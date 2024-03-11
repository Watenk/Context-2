using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mural : IFixedUpdateable
{
    private ChimeSequence chimeSequence;
    private GameObject gameObject;
    private float detectRange;

    // Mushrooms
    private List<Animator> mushrooms = new List<Animator>();
    private List<MuralMushroomPrefab> mushroomPrefabs;
    private float mushroomDistance;

    // Chimes
    private LoopingSound currentChime;
    private bool playerInRange;
    private bool playing;
    private int playingIndex;
    private Timer chimeTimer;
    private float shortChimeTime;
    private float longChimeTime;

    // References
    private PlayerController player;
    private InputHandler inputHandler;
    private SoundManager soundManager;
    private TimerManager timerManager;

    //------------------------------------

    public Mural(ChimeSequence chimeSequence, GameObject gameObject){
        this.chimeSequence = chimeSequence;
        this.gameObject = gameObject;
        player = GameManager.Instance.Player;
        detectRange = MuralSettings.Instance.DetectRange;
        mushroomPrefabs = MuralSettings.Instance.MushroomPrefabs;
        mushroomDistance = MuralSettings.Instance.MushroomDistance;
        shortChimeTime = MuralSettings.Instance.ShortChimePlayDelay;
        longChimeTime = MuralSettings.Instance.LongChimePlayDelay;
        inputHandler = GameManager.GetService<InputHandler>();
        soundManager = GameManager.GetService<SoundManager>();
        timerManager = GameManager.GetService<TimerManager>();

        inputHandler.OnChime += OnChime;
        chimeTimer = timerManager.AddTimer(shortChimeTime);

        SpawnMushrooms();

        #if UNITY_EDITOR
            if (detectRange == 0) { Debug.LogWarning("DetectRange in MuralSettings is 0"); }
        #endif
    }

    public void OnFixedUpdate(){

        if (Vector3.Distance(gameObject.transform.position, player.gameObject.transform.position) > detectRange){
            playerInRange = false;
        }
        else{
            playerInRange = true;
        } 

        if (!playing) { return; }

        if (chimeTimer.IsDone()){

            if (playingIndex == chimeSequence.chimes.Count){
                playing = false;
                mushrooms[playingIndex - 1].SetBool("Active", false);
                playingIndex = 0;
                currentChime.StopSound(); 
                currentChime = null;
                return;
            }

            PlayChime(chimeSequence.chimes[playingIndex]);
            if (chimeSequence.chimes[playingIndex].isLong){
                chimeTimer.ChangeLenght(longChimeTime);
            }
            else{
                chimeTimer.ChangeLenght(shortChimeTime);
            }
            chimeTimer.ResetTime();
            playingIndex++;
        }
    }

    //----------------------------------------

    private void OnChime(Chime chime){

        if (!playerInRange && !playing) { return; }

        chimeTimer.ResetTime();
        playing = true;
        playingIndex = 0;
    }

    private void PlayChime(Chime chime){

        if (currentChime != null) { currentChime.StopSound(); currentChime = null; }
        if (playingIndex != 0) { mushrooms[playingIndex - 1].SetBool("Active", false); }

        mushrooms[playingIndex].SetBool("Active", true);
        PlayerSoundData playerSoundData = soundManager.GetPlayerSound(chime.chimeInput);
        currentChime = soundManager.PlayLoopingSound(playerSoundData, gameObject.transform.position);
    }

    private void SpawnMushrooms(){
        Vector3 dir = gameObject.transform.right;
        Vector3 centerPoint = gameObject.transform.position;
        float totalLength = mushroomDistance * (chimeSequence.chimes.Count - 1);
        Vector3 startPosition = centerPoint - dir * totalLength / 2f;
            
        for (int i = 0; i < chimeSequence.chimes.Count; i++){
            ChimeInputs chimeInput = chimeSequence.chimes[i].chimeInput;
            Vector3 mushroomPos = startPosition + dir * mushroomDistance * i;
            GameObject newMushroom = GameObject.Instantiate(mushroomPrefabs.Find(chimePrefab => chimePrefab.chimeInputs == chimeInput).gameObject, mushroomPos, Quaternion.identity, gameObject.transform);
            if (chimeSequence.chimes[i].isLong) { newMushroom.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f); }
            Animator mushroomAnimator = newMushroom.GetComponent<Animator>();

            #if UNITY_EDITOR
                if (mushroomAnimator == null) { Debug.LogError("Mushroom doesnt contain a animator"); }
            #endif

            mushrooms.Add(mushroomAnimator);
        }
    }
}
