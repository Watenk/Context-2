using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentFollowingState : BaseState<Agent>
{
    public Action OnFollow;

    private Timer spaceTimer;
    private float followPlayerAtDistance;
    private float normalAgentSpeed;
    private float followPlayerSpeed;

    // References
    private TimerManager timerManager;
    private PlayerController player;
    private CommunityManager communityManager;

    //-----------------------------------------

    public override void OnAwake(){
        timerManager = GameManager.GetService<TimerManager>();
        communityManager = GameManager.GetService<CommunityManager>();
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
        owner.Animator.SetBool("Inactive", false);
        normalAgentSpeed = owner.NavMeshAgent.speed;
        owner.NavMeshAgent.speed = followPlayerSpeed;
        spaceTimer = timerManager.AddLoopingTimer(0.5f);
        communityManager.AddActiveAgent(owner.Group.CommunityType);
        OnFollow();
    }

    public override void OnUpdate(){
        owner.Animator.SetFloat("Speed", owner.NavMeshAgent.velocity.magnitude);
        FollowPlayer();
    }

    public override void OnExit(){
        owner.NavMeshAgent.speed = normalAgentSpeed;
        timerManager.RemoveLoopingTimer(spaceTimer);
        communityManager.RemoveActiveAgent(owner.Group.CommunityType);
    }

    //----------------------------------------

    private void FollowPlayer(){
        if (owner.DestinationReached){
            owner.SetDestination(player.transform.position, followPlayerAtDistance);
        }
    }
}
