using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentFollowingState : BaseState<Agent>
{
    private Timer spaceTimer;
    private float followPlayerAtDistance;
    private float normalAgentSpeed;
    private float followPlayerSpeed;

    // References
    private InputManager inputManager;
    private TimerManager timerManager;
    private PlayerController player;

    //-----------------------------------------

    public override void OnAwake(){
        inputManager = GameManager.Instance.GetService<InputManager>();
        timerManager = GameManager.Instance.GetService<TimerManager>();
        player = GameManager.Instance.Player;
        followPlayerAtDistance = AgentSettings.Instance.FollowPlayerAtDistance;
        followPlayerSpeed = AgentSettings.Instance.FollowPlayerSpeed;

        #if UNITY_EDITOR
            if (player == null) { Debug.LogError("Couldn't get player from GameManager"); }
            if (followPlayerAtDistance == 0) { Debug.LogError("followPlayerAtDistance is 0 in AgentSettings"); }
            if (followPlayerSpeed == 0) { Debug.LogError("followPlayerSpeed is 0 in AgentSettings"); }
        #endif
    }

    public override void OnStart(){
        normalAgentSpeed = owner.NavMeshAgent.speed;
        owner.NavMeshAgent.speed = followPlayerSpeed;
        spaceTimer = timerManager.AddTimer(0.5f);
        inputManager.OnSpace += OnSpace;
    }

    public override void OnUpdate(){
        SwitchState();
        FollowPlayer();
    }

    public override void OnExit(){
        owner.NavMeshAgent.speed = normalAgentSpeed;
        timerManager.RemoveTimer(spaceTimer);
        inputManager.OnSpace -= OnSpace;
    }

    //----------------------------------------

    private void OnSpace(){
        spaceTimer.ChangeTime(0.5f);
    }

    private void FollowPlayer(){
        if (owner.DestinationReached){
            owner.SetDestination(player.transform.position, followPlayerAtDistance);
        }
    }

    private void SwitchState(){
        // if (spaceTimer.IsDone()){
        //    owner.fsm.SwitchState(typeof(AgentLookAtPlayerState));
        // }
    }
}
