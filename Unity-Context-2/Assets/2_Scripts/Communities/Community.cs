using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Community : IFixedUpdateable
{
    public CommunityTypes CommunityType { get; private set; }
    private List<Group> groups = new List<Group>();
    private List<Problem> problems = new List<Problem>();
    // TODO: Add Affection for other communities

    // References
    private ChimeSequencer chimeSequencer;

    //------------------------------------------------

    public Community(CommunityTypes communityType){
        chimeSequencer = GameManager.GetService<ChimeSequencer>();
        this.CommunityType = communityType;

        chimeSequencer.OnChimeSequence += OnChimeSequence;
    }

    public void OnFixedUpdate(){
        foreach (Group currentGroup in groups){
            currentGroup.OnFixedUpdate();
        }
    }

    public void AddGroup(int size, Vector3 pos, float spawnRadius){
        groups.Add(new Group(this, CommunityType, size, pos, spawnRadius));
    }

    public void AddProblem(Problem problem){
        problems.Add(problem);
    }

    public List<CommunityTypes> GetFollowingAgents(){
        
        if (groups.Count == 0) { return default; }
        List<CommunityTypes> followingAgents = new List<CommunityTypes>();

        foreach (Group currentGroup in groups){
            followingAgents.AddRange(currentGroup.GetFollowingAgents());
        }

        return followingAgents;
    }

    //--------------------------------------------------

    private void OnChimeSequence(ChimeSequence chimeSequence){
        if (chimeSequence.affectedCommunities.Contains(CommunityType)){
            foreach (Group currentGroup in groups){
                currentGroup.ExecuteTask(chimeSequence.chimeTask);
            }
        }
    }
}
