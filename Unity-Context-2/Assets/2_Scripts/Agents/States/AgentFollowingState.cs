using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentFollowingState : BaseState<Agent>
{
    public Action<CommunityTypes> OnFollow;

    private Timer spaceTimer;
    private float followPlayerAtDistance;
    private float normalAgentSpeed;
    private float followPlayerSpeed;

    // References
    private TimerManager timerManager;
    private PlayerController player;
    private CommunityManager communityManager;
    private SoundManager soundManager;

    //-----------------------------------------

    public override void OnAwake(){
        timerManager = GameManager.GetService<TimerManager>();
        communityManager = GameManager.GetService<CommunityManager>();
        soundManager = GameManager.GetService<SoundManager>();
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
        OnFollow(owner.Group.CommunityType);
    }

    public override void OnUpdate(){
        owner.Animator.SetFloat("Speed", owner.NavMeshAgent.velocity.magnitude);
        owner.Animator.SetFloat("Mult", owner.NavMeshAgent.velocity.magnitude / 2f);
        FollowPlayer();

        // Stop Following
        if (player.CurrentCommunity != owner.Group.CommunityType && player.CurrentCommunity != CommunityTypes.global){
            if (communityManager.IsLibraryProblemSolved(owner.Group.CommunityType)) return;

            NPCSoundData soundData = soundManager.GetNPCSound(owner.Group.CommunityType, ChimeTasks.follow);
            soundManager.PlayNPCSound(soundData, false, owner.GameObject.transform.position);
            fsm.SwitchState(typeof(AgentWanderingState));
        }
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
