using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChimeFollowState : BaseState<ChimeManager>
{
    public override void OnStart(){
        Debug.Log("FollowState");
    }
}
