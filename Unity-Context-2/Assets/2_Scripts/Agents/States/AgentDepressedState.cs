using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentDepressedState : BaseState<Agent>
{
    public override void OnStart(){
        owner.Animator.SetFloat("Speed", 0.0f);
        owner.Animator.SetBool("Inactive", true);
    }

    public override void OnExit(){
        owner.Animator.SetBool("Inactive", false);
    }
}
