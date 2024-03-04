using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentLookAtPlayerState : BaseState<Agent>
{
    private float agentLookAtPlayerDistance;

    // References
    private PlayerController player;

    //---------------------------------------

    public override void OnAwake(){
        agentLookAtPlayerDistance = AgentSettings.Instance.LookAtPlayerDistance;
        player = GameManager.Instance.Player;

        #if UNITY_EDITOR
            if (agentLookAtPlayerDistance == 0) { Debug.LogError("agentLookAtPlayerDistance is 0 in AgentSettings"); }
            if (player == null) { Debug.LogError("Couldn't get player from GameManager"); }
        #endif
    }

    public override void OnUpdate(){
        RotateTowardsPlayer();
        CheckState();
    }

    public override void OnExit(){
        owner.NavMeshAgent.isStopped = false;
    }

    //-------------------------------------

    private void RotateTowardsPlayer(){
        Vector3 directionToPlayer = player.transform.position - owner.GameObject.transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
        owner.GameObject.transform.rotation = Quaternion.Slerp(owner.GameObject.transform.rotation, targetRotation, 1.0f * Time.deltaTime);
    }

    private void CheckState(){
        if (Vector3.Distance(owner.GameObject.transform.position, player.transform.position) > agentLookAtPlayerDistance){
            owner.fsm.SwitchState(typeof(AgentWanderingState));
        }
    }
}
