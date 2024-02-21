using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentWanderingState : BaseState<Agent>
{
    private float wanderFromHomeDistance = 0;

    //---------------------------------------------

    public override void OnAwake(){
        wanderFromHomeDistance = AgentSettings.Instance.WanderFromHomeDistance;

        #if UNITY_EDITOR
            if (wanderFromHomeDistance == 0) { Debug.LogError("wanderFromHomeDistance is 0 in AgentSettings"); }
        #endif
    }

    public override void OnUpdate(){
        if (owner.DestinationReached){
            Vector3 newDestination = new Vector3(owner.Group.Home.x + Random.Range(-wanderFromHomeDistance, wanderFromHomeDistance), 0, owner.Group.Home.z + Random.Range(-wanderFromHomeDistance, wanderFromHomeDistance));
            owner.SetDestination(newDestination);
        }
    }
}
