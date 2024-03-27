using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProblemSpawner : MonoBehaviour
{
    public CommunityTypes CommunityType;
    [Tooltip("Groups that will be freed after the problem is solved")]
    public List<GroupSpawner> FreedGroups;
    [Tooltip("What kind and how many community members are needed to solve this problem")]
    public List<CommunityTypes> ProblemSolvers;
    public bool LibraryProblem;

    private List<Group> groups = new List<Group>();

    //-----------------------------------------------

    public void Start(){

        foreach (GroupSpawner current in FreedGroups){
            groups.Add(current.GetGroup());
        }

        GameManager.GetService<CommunityManager>().AddProblem(CommunityType, new Problem(ProblemSolvers, CommunityType, groups, this.gameObject, transform.position, LibraryProblem));
        Destroy(this);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position, 1);
    }
}
