using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChimeSequencer : IFixedUpdateable
{
    public event Action<ChimeSequence> OnChimeSequence;

    private List<Chime> currentChimes = new List<Chime>();
    private List<ChimeSequence> chimeSequences = new List<ChimeSequence>();
    private Timer chimeResetTimer;
    private bool playerIsEntering;

    // References
    private InputHandler inputManager;
    private TimerManager timerManager;

    //----------------------------------------------

    public ChimeSequencer(){
        inputManager = GameManager.GetService<InputHandler>();
        timerManager = GameManager.GetService<TimerManager>();

        chimeResetTimer = timerManager.AddLoopingTimer(ChimeSettings.Instance.ChimeResetTime);
        chimeSequences = ChimeSettings.Instance.ChimeSequences;

        inputManager.OnChime += OnChime;
    }

    public void OnFixedUpdate(){
        if (playerIsEntering && chimeResetTimer.IsDone()){
            ClearSequence();
        }
    }

    public ChimeSequence GetChimeSequence(ChimeTasks chimeSequence){
        foreach (ChimeSequence currentSequence in chimeSequences){
            if (currentSequence.chimeTask == chimeSequence){
                return currentSequence;
            }
        }

        #if UNITY_EDITOR
            Debug.LogError("GetChimeSequence didn't find the" + chimeSequence.ToString()  +  " chime in chimeSequences. Check the chimes in chimeSettings");
        #endif

        return null;
    }

    //------------------------------------------------

    private void OnChime(Chime chime){

        currentChimes.Add(chime);
        playerIsEntering = true;
        chimeResetTimer.Interrupt();

        foreach (ChimeSequence currentSequence in chimeSequences){
            if (ChimeSequencesAreEqual(currentChimes, currentSequence.chimes)){
                OnChimeSequence?.Invoke(currentSequence);
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
                return false;
            }
        }
        return true;
    }
}
