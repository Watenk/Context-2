using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ChimeManager : IFixedUpdateable
{
    private List<Chime> currentChimes = new List<Chime>();
    private List<TaskChime> taskChimes = new List<TaskChime>();
    private Fsm<ChimeManager> fsm;
    private Timer chimeResetTimer;

    // References
    private InputManager inputManager;
    private TimerManager timerManager;

    //----------------------------------------------

    public ChimeManager(){
        inputManager = GameManager.GetService<InputManager>();
        timerManager = GameManager.GetService<TimerManager>();

        chimeResetTimer = timerManager.AddTimer(ChimeSettings.Instance.ChimeResetTime);
        taskChimes = ChimeSettings.Instance.taskChimes;
        fsm = new Fsm<ChimeManager>(this,
            new ChimeIdleState(),
            new ChimeFollowState()
        );
        fsm.SwitchState(typeof(ChimeIdleState));

        inputManager.OnChime += OnChime;
    }

    public void OnFixedUpdate(){
        fsm.OnUpdate();

        if (chimeResetTimer.IsDone()){
            currentChimes.Clear();
            fsm.SwitchState(typeof(ChimeIdleState));
        }
    }

    //------------------------------------------------

    private void OnChime(Chime chime){

        if (fsm.currentState == fsm.GetState(typeof(ChimeIdleState))){
            currentChimes.Add(chime);
            chimeResetTimer.ResetTime();

            foreach (TaskChime current in taskChimes){
                if (AreChimeSequencesEqual(currentChimes, current.chimes)){
                    SetState(current.chimeTask);
                }
            }
        }
    }

    private bool AreChimeSequencesEqual(List<Chime> list1, List<Chime> list2)
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

    private void SetState(ChimeTasks chimeTask){

        switch (chimeTask){
            case ChimeTasks.followChime:
                fsm.SwitchState(typeof(ChimeFollowState));
                break;
        }
    }


    // public event Action<Chime> OnChime;

    // private List<Chime> chimeInputs = new List<Chime>();
    // private Fsm<ChimeManager> fsm;
    // private Timer chimeResetTimer;
    // private Timer longChimeTimer;

    // private GameObject globalSound;
    // private GameObject squareSound;
    // private GameObject triangleSound;
    // private GameObject circleSound;

    // // References
    // private InputManager inputManager;
    // private TimerManager timerManager;
    // private AudioManager audioManager;

    // //--------------------------------------------

    // public ChimeManager(){
    //     timerManager = GameManager.GetService<TimerManager>();
    //     inputManager = GameManager.GetService<InputManager>();
    //     audioManager = GameManager.GetService<AudioManager>();

    //     chimeResetTimer = timerManager.AddTimer(ChimeSettings.Instance.ChimeResetTime);
    //     longChimeTimer = timerManager.AddTimer(ChimeSettings.Instance.LongChimeTime);

    //     fsm = new Fsm<ChimeManager>(this,
    //         new ChimeFollowState()
    //     );

    //     inputManager.OnGlobalDown += OnGlobalDown;
    //     inputManager.OnGlobalUp += OnGlobalUp;
    //     inputManager.OnSquareDown += OnSquareDown;
    //     inputManager.OnSquareUp += OnSquareUp;
    //     inputManager.OnCircleDown += OnCircleDown;
    //     inputManager.OnCircleUp += OnCircleUp;
    //     inputManager.OnTriangleDown += OnTriangleDown;
    //     inputManager.OnTriangleUp += OnTriangleUp;
    // }

    // public void OnFixedUpdate(){
    //     if (chimeResetTimer.IsDone()){
    //         chimeInputs.Clear();
    //     }

    //     fsm.OnUpdate();
    // }

    // //--------------------------------------------

    // private void ChimeInputDown(ChimeInputs chimeInput){
    //     longChimeTimer.ResetTime();

    //     // Chime sounds
    //     switch (chimeInput){
    //         case ChimeInputs.global:
    //             globalSound = audioManager.PlaySound(AudioSettings.Instance.PlayPlayerGlobalChime, GameManager.Player.gameObject.transform.position);
    //             break;
    //         case ChimeInputs.triangle:
    //             triangleSound = audioManager.PlaySound(AudioSettings.Instance.PlayPlayerTriangleChime, GameManager.Player.gameObject.transform.position);
    //             break;
    //         case ChimeInputs.circle:
    //             circleSound = audioManager.PlaySound(AudioSettings.Instance.PlayPlayerCircleChime, GameManager.Player.gameObject.transform.position);
    //             break;
    //         case ChimeInputs.square:
    //             squareSound = audioManager.PlaySound(AudioSettings.Instance.PlayPlayerSquareChime, GameManager.Player.gameObject.transform.position);
    //             break;
    //     }
    // }

    // private void ChimeInputUp(ChimeInputs chimeInput){

    //     if (longChimeTimer.IsDone()){
    //         if (OnChime != null) { OnChime(new Chime(chimeInput, true)); }
    //     }
    //     else{
    //         if (OnChime != null) { OnChime(new Chime(chimeInput, false)); }
    //     }

    //     // Chime sounds
    //     switch (chimeInput){
    //         case ChimeInputs.global:
    //             audioManager.StopSound(globalSound);
    //             break;
    //         case ChimeInputs.triangle:
    //             audioManager.StopSound(triangleSound);
    //             break;
    //         case ChimeInputs.circle:
    //             audioManager.StopSound(circleSound);
    //             break;
    //         case ChimeInputs.square:
    //             audioManager.StopSound(squareSound);
    //             break;
    //     }


    // }

    // private void OnGlobalDown(){
    //     ChimeInputDown(ChimeInputs.global);
    // }

    // private void OnSquareDown(){
    //     ChimeInputDown(ChimeInputs.square);
    // }

    // private void OnCircleDown(){
    //     ChimeInputDown(ChimeInputs.circle);
    // }

    // private void OnTriangleDown(){
    //     ChimeInputDown(ChimeInputs.triangle);
    // }

    // private void OnGlobalUp(){
    //     ChimeInputUp(ChimeInputs.global);
    // }

    // private void OnSquareUp(){
    //     ChimeInputUp(ChimeInputs.square);
    // }

    // private void OnCircleUp(){
    //     ChimeInputUp(ChimeInputs.circle);
    // }

    // private void OnTriangleUp(){
    //     ChimeInputUp(ChimeInputs.triangle);
    // }
}
