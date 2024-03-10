using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProblemSpawner : MonoBehaviour
{
    public CommunityTypes CommunityType;
    [Tooltip("Amount of npc's in the community that will be freed (go into wander state from depressed state)")]
    public int FreeNPCAmount;
    [Tooltip("What kind and how many community members are needed to solve this problem")]
    public List<CommunityTypes> ProblemSolvers;

    //-----------------------------------------------

    public void Start(){
        GameManager.GetService<CommunityManager>().AddProblem(CommunityType, new Problem(ProblemSolvers, CommunityType, FreeNPCAmount, this.gameObject, transform.position));
        Destroy(this);
    }
}
