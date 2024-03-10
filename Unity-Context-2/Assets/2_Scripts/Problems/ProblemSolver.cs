using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProblemSolver
{
    public CommunityTypes CommunityType { get; private set; }
    public GameObject Mushroom { get; private set; } 
    public Animator Animator { get; private set; } 
    public bool IsSolved;
 
    //---------------------------------------------

    public ProblemSolver(CommunityTypes communityType, GameObject mushroom){
        this.CommunityType = communityType;
        this.Mushroom = mushroom;
        this.Animator = mushroom.GetComponent<Animator>();

        #if UNITY_EDITOR
            if (Animator == null) { Debug.LogError("Couldn't find animator on mushroom prefab of type " + CommunityType.ToString()); }
        #endif
    }
}
