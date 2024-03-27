using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentWanderingState : BaseState<Agent>
{
    private float wanderFromHomeDistance;
    private float lookAtPlayerDistance;

    // References
    private PlayerController player;

    //---------------------------------------------

    public override void OnAwake(){
        lookAtPlayerDistance = AgentSettings.Instance.LookAtPlayerDistance;
        player = GameManager.Instance.Player;
        wanderFromHomeDistance = owner.Group.WanderFromHomeDistance;

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
            owner.NavMeshAgent.speed = 15;
        }
        else{
            owner.NavMeshAgent.speed = owner.communityManager.GetSpeed(owner.Group.CommunityType);
        }
    }

    private void Wander(){
        if (owner.DestinationReached){
            Vector3 newDestination = new Vector3(owner.Group.Home.x + Random.Range(-wanderFromHomeDistance, wanderFromHomeDistance), 0, owner.Group.Home.z + Random.Range(-wanderFromHomeDistance, wanderFromHomeDistance));
            owner.SetDestination(newDestination, 0.5f);
        }
    }
}
