using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChimeFollowState : BaseState<ChimeManager>
{
    private Timer selectionTimer;

    // References
    private TimerManager timerManager;
    private InputManager inputManager;

    //-----------------------------------

    public override void OnAwake(){
        timerManager = GameManager.GetService<TimerManager>();
        inputManager = GameManager.GetService<InputManager>();

        selectionTimer = timerManager.AddTimer(ChimeSettings.Instance.ChimeResetTime);
    }

    public override void OnStart(){
        inputManager.OnChime += OnChime;
    }

    public override void OnExit(){
        inputManager.OnChime -= OnChime;
    }

    //------------------------------------

    private void OnChime(Chime chime){
        
    }
}
