using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Community : IFixedUpdateable
{
    public Action<CommunityTypes> OnFollow;
    public CommunityTypes CommunityType { get; private set; }
    private List<Group> groups = new List<Group>();
    private List<Problem> problems = new List<Problem>();
    private List<Problem> problemsGC = new List<Problem>();
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

        foreach (Problem currentProblem in problemsGC){
            currentProblem.Remove();
            problems.Remove(currentProblem);
        }
        problemsGC.Clear();
    }

    public Group AddGroup(int size, Vector3 pos, float spawnRadius, bool isActive){
        Group newGroup = new Group(this, CommunityType, size, pos, spawnRadius, isActive);
        groups.Add(newGroup);
        newGroup.OnFollow += Follow;
        return newGroup;
    }

    public void AddProblem(Problem problem){
        problems.Add(problem);
    }

    public void RemoveProblem(Problem problem){
        problemsGC.Add(problem);
    }

    public void ProblemSolved(){
        foreach (Group currentGroup in groups){
            currentGroup.ProblemSolved();
        }
    }

    private void Follow(CommunityTypes communityType){
        OnFollow(communityType);
    }
}
