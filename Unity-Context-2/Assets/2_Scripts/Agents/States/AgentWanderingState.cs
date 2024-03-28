using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentWanderingState : BaseState<Agent>
{
    private float wanderFromHomeDistance;
    private float lookAtPlayerDistance;
    private bool isInLib;
    private Timer stayInLibTimer;

    // References
    private PlayerController player;
    private CommunityManager communityManager;
    private TimerManager timerManager;

    //---------------------------------------------

    public override void OnAwake(){
        lookAtPlayerDistance = AgentSettings.Instance.LookAtPlayerDistance;
        player = GameManager.Instance.Player;
        wanderFromHomeDistance = owner.Group.WanderFromHomeDistance;
        communityManager = GameManager.GetService<CommunityManager>();
        timerManager = GameManager.GetService<TimerManager>();

        stayInLibTimer = timerManager.AddTimer(Random.Range(10.0f, 20.0f));

        #if UNITY_EDITOR
            if (lookAtPlayerDistance == 0) { Debug.LogError("LookAtPlayerDistance is 0 in AgentSettings"); }
            if (player == null) { Debug.LogError("Couldn't get player from GameManager"); }
        #endif
    }

    public override void OnStart()
    {
        owner.Animator.SetBool("Inactive", false);
    }

    public override void OnUpdate(){
        owner.Animator.SetFloat("Speed", owner.NavMeshAgent.velocity.magnitude);
        owner.Animator.SetFloat("Mult", owner.NavMeshAgent.velocity.magnitude / 2f);
        CheckState();
        Wander();
    }

    public override void OnExit(){
        owner.SetDestination(owner.GameObject.transform.position, 0.5f);
        owner.NavMeshAgent.isStopped = true;
    }

    //---------------------------------------------

    private void CheckState(){
        if (Vector3.Distance(owner.GameObject.transform.position, player.transform.position) < lookAtPlayerDistance){
            owner.fsm.SwitchState(typeof(AgentLookAtPlayerState));
        }

        if (Vector3.Distance(owner.GameObject.transform.position, owner.Group.Home) > owner.Group.WanderFromHomeDistance){
            if (isInLib && Vector3.Distance(new Vector3(35.0f, 0.0f, -40.0f), owner.GameObject.transform.position) < 20.0f) owner.NavMeshAgent.speed = owner.communityManager.GetSpeed(owner.Group.CommunityType);
            else owner.NavMeshAgent.speed = 15;
            
        }
        else{
            owner.NavMeshAgent.speed = owner.communityManager.GetSpeed(owner.Group.CommunityType);
        }
    }

    private void Wander(){
        if (owner.DestinationReached){

            if (stayInLibTimer.IsDone()) isInLib = false;

            if (isInLib){
                Vector3 newDestination = new Vector3(35.0f + Random.Range(-15, 15), 0, -40.0f + Random.Range(-15, 15)); // Walk to lib
                owner.SetDestination(newDestination, 0.5f);
            }
            else{
                Vector3 newDestination;
                if (communityManager.IsLibraryProblemSolved(owner.Group.CommunityType) && Random.Range(0, 100) < AgentSettings.Instance.WalkToLibChance){
                    newDestination = new Vector3(35.0f + Random.Range(-15, 15), 0, -40.0f + Random.Range(-15, 15)); // Walk to lib
                    isInLib = true;
                    stayInLibTimer.Reset();
                }
                else{
                    newDestination = new Vector3(owner.Group.Home.x + Random.Range(-wanderFromHomeDistance, wanderFromHomeDistance), 0, owner.Group.Home.z + Random.Range(-wanderFromHomeDistance, wanderFromHomeDistance));
                }

                owner.SetDestination(newDestination, 0.5f);
            }
        }
    }
}
