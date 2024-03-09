using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Community : IFixedUpdateable
{
    public CommunityTypes CommunityType { get; private set; }
    private List<Group> groups = new List<Group>();
    private List<Problem> problems = new List<Problem>();
    // TODO: Add Affection for other communities

    //------------------------------------------------

    public Community(CommunityTypes communityType){
        this.CommunityType = communityType;
    }

    public void OnFixedUpdate(){
        foreach (Group currentGroup in groups){
            currentGroup.OnFixedUpdate();
        }

        foreach (Problem currentProblem in problems){
            currentProblem.OnFixedUpdate();
        }
    }

    public void AddGroup(int size, Vector3 pos, float spawnRadius){
        groups.Add(new Group(this, CommunityType, size, pos, spawnRadius));
    }

    public void AddProblem(Problem problem){
        problems.Add(problem);
    }
}
