using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommunityManager : IFixedUpdateable
{
    private Dictionary<CommunityTypes, int> activeAgents = new Dictionary<CommunityTypes, int>();
    private Dictionary<CommunityTypes, Community> communities = new Dictionary<CommunityTypes, Community>();

    //-------------------------------------------

    public CommunityManager(){

        Add(CommunityTypes.circle);
        Add(CommunityTypes.triangle);
        Add(CommunityTypes.square);
    }

    public void OnFixedUpdate(){
        foreach (KeyValuePair<CommunityTypes, Community> kvp in communities){
            kvp.Value.OnFixedUpdate();
        }
    }

    public Community GetCommunity(CommunityTypes communityType){
        communities.TryGetValue(communityType, out Community community);

        #if UNITY_EDITOR
            if (community == null) { Debug.LogError("Tried to get " + communityType.ToString() + " from communities but its null"); }
        #endif

        return community;
    }

    public List<CommunityTypes> GetCommunityAmount(){

        List<CommunityTypes> keys = new List<CommunityTypes>();

        foreach (KeyValuePair<CommunityTypes, Community> kvp in communities){
            keys.Add(kvp.Key);
        }

        return keys;
    }

    public Dictionary<CommunityTypes, int> GetActiveAgents(){
        return activeAgents;
    }

    public void AddActiveAgent(CommunityTypes communityType){
        activeAgents[communityType] += 1;
    }

    public void RemoveActiveAgent(CommunityTypes communityType){
        activeAgents[communityType] -= 1;
    }

    public void AddGroup(CommunityTypes communityType, int size, Vector3 pos, float spawnRadius, bool isActive){
        GetCommunity(communityType).AddGroup(size, pos, spawnRadius, isActive);
    }

    public void AddProblem(CommunityTypes communityType, Problem problem){
        GetCommunity(communityType).AddProblem(problem);
    }

    public void RemoveProblem(CommunityTypes communityType, Problem problem){
        GetCommunity(communityType).RemoveProblem(problem);

        foreach (KeyValuePair<CommunityTypes, Community> kvp in communities){
            kvp.Value.ProblemSolved();
        }
    }

    //----------------------------------------------

    private void Add(CommunityTypes communityType){
        communities.Add(communityType, new Community(communityType));
        activeAgents.Add(communityType, 0);
    }
}
