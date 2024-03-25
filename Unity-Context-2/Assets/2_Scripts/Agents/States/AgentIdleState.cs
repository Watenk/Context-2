using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentIdleState : BaseState<Agent>
{
    public override void OnStart(){
        owner.Animator.SetFloat("Speed", 0.0f);
    }
}
