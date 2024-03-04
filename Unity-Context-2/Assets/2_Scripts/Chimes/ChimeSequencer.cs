using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChimeSequencer
{
    public event Action<ChimeSequences> OnChimeSequence;

    private List<Chime> currentChimes = new List<Chime>();
    private List<ChimeSequence> ChimeSequences = new List<ChimeSequence>();
    private Timer chimeResetTimer;
    private bool playerIsEntering;

    // References
    private InputHandler inputManager;
    private TimerManager timerManager;

    //----------------------------------------------

    public ChimeSequencer(){
        inputManager = GameManager.GetService<InputHandler>();
        timerManager = GameManager.GetService<TimerManager>();

        chimeResetTimer = timerManager.AddTimer(ChimeSettings.Instance.ChimeResetTime);
        ChimeSequences = ChimeSettings.Instance.ChimeSequences;

        inputManager.OnChime += OnChime;
    }

    public void OnFixedUpdate(){
        if (playerIsEntering && chimeResetTimer.IsDone()){
            ClearSequence();
        }
    }

    //------------------------------------------------

    private void OnChime(Chime chime){

        currentChimes.Add(chime);
        playerIsEntering = true;
        chimeResetTimer.ResetTime();

        foreach (ChimeSequence currentSequence in ChimeSequences){
            if (ChimeSequencesAreEqual(currentChimes, currentSequence.chimes)){
                OnChimeSequence(currentSequence.chimeSequence);
                ClearSequence();
            }
        }
    }

    private void ClearSequence(){
        currentChimes.Clear();
        playerIsEntering = false;
    }

    private bool ChimeSequencesAreEqual(List<Chime> list1, List<Chime> list2)
    {
        if (list1.Count != list2.Count){
            return false;
        }

        for (int i = 0; i < list1.Count; i++)
        {
            ChimeInputs chimeInput1 = list1[i].chimeInput;         
            ChimeInputs chimeInput2 = list2[i].chimeInput;   
            bool chime1IsLong = list1[i].isLong;            
            bool chime2IsLong = list2[i].isLong;   

            if (chimeInput1 != chimeInput2){
                return false;
            }         

            if (chime1IsLong != chime2IsLong){
                Debug.Log(chime1IsLong + ", " + chime2IsLong);
                return false;
            }
        }
        return true;
    }
}
