using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Agent : IFixedUpdateable
{
    public Action<CommunityTypes> OnFollow;

    public GameObject GameObject { get; private set; }
    public Group Group { get; private set; }
    public bool DestinationReached { get; private set; }
    public NavMeshAgent NavMeshAgent { get; private set; }
    public Fsm<Agent> fsm { get; private set; }
    public Animator Animator { get; private set; }
    public ParticleSystem ConfusedParticleSystem { get; private set; }
    public ParticleSystem TroubledParticleSystem { get; private set; }
    public BubbleController BubbleController { get; private set; }

    // Pathfinding
    private float wanderFromHomeDistance;

    // References
    private ChimeSequencer chimeSequencer;
    private SoundManager soundManager;
    public CommunityManager communityManager { get; private set; }

    //----------------------------------------

    public Agent(GameObject gameObject, Group group, Animator animator, ParticleSystem[] particleSystems, BubbleController bubbleController){
        GameObject = gameObject;
        Group = group;
        Animator = animator;
        ConfusedParticleSystem = particleSystems[0];
        TroubledParticleSystem = particleSystems[1];
        BubbleController = bubbleController;
        communityManager = GameManager.GetService<CommunityManager>();
        NavMeshAgent = GameManager.GetComponent<NavMeshAgent>(GameObject);
        chimeSequencer = GameManager.GetService<ChimeSequencer>();
        soundManager = GameManager.GetService<SoundManager>();
        wanderFromHomeDistance = Group.WanderFromHomeDistance;;
        NavMeshAgent.speed = communityManager.GetSpeed(Group.CommunityType);
        DestinationReached = true;

        chimeSequencer.OnChimeSequence += OnChimeSequence;

        fsm = new Fsm<Agent>(this,
           new AgentIdleState(),
           new AgentFollowingState(),
           new AgentWanderingState(),
           new AgentLookAtPlayerState(),
           new AgentDepressedState()
        );
        fsm.SwitchState(typeof(AgentDepressedState));

        ((AgentFollowingState)fsm.GetState(typeof(AgentFollowingState))).OnFollow += Follow;

        #if UNITY_EDITOR
            if (NavMeshAgent == null) { Debug.LogError(gameObject.name + " doesn't contain a navmeshAgent"); }
        #endif

        EventManager.AddListener<CommunityTypes>(Events.OnCommunitySpeedChange,(communityType) => UpdateSpeed(communityType));
    }

    public void OnFixedUpdate(){
        fsm.OnUpdate();
        if (NavMeshAgent.remainingDistance <= NavMeshAgent.stoppingDistance) DestinationReached = true;
    }

    public void SetDestination(Vector3 pos, float stopDistance){
        NavMeshAgent.stoppingDistance = stopDistance;
        NavMeshAgent.SetDestination(pos);
        DestinationReached = false;
    }

    //------------------------------------------

    private void UpdateSpeed(CommunityTypes communityType){
        if (communityType != Group.CommunityType) return;

        NavMeshAgent.speed = communityManager.GetSpeed(communityType);
    }

    private void Follow(CommunityTypes communityType){
        OnFollow(communityType);
    }

    private void OnChimeSequence(ChimeSequence chimeSequence){
        if(chimeSequence.chimeTask == ChimeTasks.Confused && fsm.currentState == fsm.GetState(typeof(AgentLookAtPlayerState)))
        {
            ConfusedParticleSystem.Play();
        }

        if (!chimeSequence.affectedCommunities.Contains(Group.CommunityType)) { return; } // If community is affected

        ExecuteTask(chimeSequence);
    }

    private void ExecuteTask(ChimeSequence chimeSequence){

        switch (chimeSequence.chimeTask){
            case ChimeTasks.follow:
                if (fsm.currentState == fsm.GetState(typeof(AgentLookAtPlayerState))){
                    fsm.SwitchState(typeof(AgentFollowingState));

                    // Sound
                    NPCSoundData soundData = soundManager.GetNPCSound(Group.CommunityType, ChimeTasks.follow);
                    soundManager.PlayNPCSound(soundData, false, GameObject.transform.position);
                    BubbleController.StartBubble(Group.CommunityType,0.5f);
                }
                break;
            
            case ChimeTasks.solveProblem:
                if (fsm.currentState == fsm.GetState(typeof(AgentFollowingState))){

                    // Sound
                    NPCSoundData soundData = soundManager.GetNPCSound(Group.CommunityType, ChimeTasks.follow);
                    soundManager.PlayNPCSound(soundData, false, GameObject.transform.position);
                }
                break;

            case ChimeTasks.leave:
                    fsm.currentState = fsm.GetState(typeof(AgentWanderingState));
                break;
        }
    }
}
