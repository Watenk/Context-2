using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProblemSpawner : MonoBehaviour
{
    public CommunityTypes communityType;
    [Tooltip("What kind and how many community members are needed to solve this problem")]
    public List<CommunityTypes> problemSolvers;

    //-----------------------------------------------

    public void Start(){
        GameManager.GetService<CommunityManager>().AddProblem(communityType, new Problem(problemSolvers, this.gameObject, transform.position));
        Destroy(this);
    }
}
