using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentWanderingState : BaseState<Agent>
{
    private float wanderFromHomeDistance = 0;
    private float lookAtPlayerDistance;

    // References
    private PlayerController player;

    //---------------------------------------------

    public override void OnAwake(){
        wanderFromHomeDistance = AgentSettings.Instance.WanderFromHomeDistance;
        lookAtPlayerDistance = AgentSettings.Instance.LookAtPlayerDistance;
        player = GameManager.Instance.Player;

        #if UNITY_EDITOR
            if (wanderFromHomeDistance == 0) { Debug.LogError("wanderFromHomeDistance is 0 in AgentSettings"); }
            if (lookAtPlayerDistance == 0) { Debug.LogError("LookAtPlayerDistance is 0 in AgentSettings"); }
            if (player == null) { Debug.LogError("Couldn't get player from GameManager"); }
        #endif
    }

    public override void OnUpdate(){
        owner.Animator.SetFloat("Speed", owner.NavMeshAgent.velocity.magnitude);
        CheckState();
        Wander();
    }

    public override void OnExit(){
        owner.SetDestination(owner.GameObject.transform.position, 0.1f);
        owner.NavMeshAgent.isStopped = true;
    }

    //---------------------------------------------

    private void CheckState(){
        if (Vector3.Distance(owner.GameObject.transform.position, player.transform.position) < lookAtPlayerDistance){
            owner.fsm.SwitchState(typeof(AgentLookAtPlayerState));
        }
    }

    private void Wander(){
        if (owner.DestinationReached){
            Vector3 newDestination = new Vector3(owner.Group.Home.x + Random.Range(-wanderFromHomeDistance, wanderFromHomeDistance), 0, owner.Group.Home.z + Random.Range(-wanderFromHomeDistance, wanderFromHomeDistance));
            owner.SetDestination(newDestination, 0.1f);
        }
    }
}
