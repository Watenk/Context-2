using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentWanderingState : BaseState<Agent>
{
    private float wanderFromHomeDistance = 0;
    private float agentLookAtPlayerDistance;

    // References
    private PlayerController player;

    //---------------------------------------------

    public override void OnAwake(){
        wanderFromHomeDistance = AgentSettings.Instance.WanderFromHomeDistance;
        agentLookAtPlayerDistance = AgentSettings.Instance.AgentLookAtPlayerDistance;
        player = GameManager.Instance.Player;

        #if UNITY_EDITOR
            if (wanderFromHomeDistance == 0) { Debug.LogError("wanderFromHomeDistance is 0 in AgentSettings"); }
            if (agentLookAtPlayerDistance == 0) { Debug.LogError("agentLookAtPlayerDistance is 0 in AgentSettings"); }
            if (player == null) { Debug.LogError("Couldn't get player from GameManager"); }
        #endif
    }

    public override void OnUpdate(){
        
        CheckState();
        SetDestination();
    }

    public override void OnExit(){
        owner.SetDestination(owner.transform.position);
        owner.NavMeshAgent.isStopped = true;
    }

    //---------------------------------------------

    private void CheckState(){
        if (Vector3.Distance(owner.transform.position, player.transform.position) < agentLookAtPlayerDistance){
            owner.fsm.SwitchState(typeof(AgentLookAtPlayerState));
        }
    }

    private void SetDestination(){
        if (owner.DestinationReached){
            Vector3 newDestination = new Vector3(owner.Group.Home.x + Random.Range(-wanderFromHomeDistance, wanderFromHomeDistance), 0, owner.Group.Home.z + Random.Range(-wanderFromHomeDistance, wanderFromHomeDistance));
            owner.SetDestination(newDestination);
        }
    }
}
