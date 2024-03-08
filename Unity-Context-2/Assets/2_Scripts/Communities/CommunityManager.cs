using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommunityManager : IFixedUpdateable
{
    private List<CommunityTypes> activeAgents = new List<CommunityTypes>();
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

    public List<CommunityTypes> GetActiveAgents(){
        return activeAgents;
    }

    public void AddActiveAgent(CommunityTypes communityType){
        activeAgents.Add(communityType);
    }

    public void RemoveActiveAgent(CommunityTypes communityType){
        activeAgents.Remove(communityType);
    }

    public void AddGroup(CommunityTypes communityType, int size, Vector3 pos, float spawnRadius){
        GetCommunity(communityType).AddGroup(size, pos, spawnRadius);
    }

    public void AddProblem(CommunityTypes communityType, Problem problem){
        GetCommunity(communityType).AddProblem(problem);
    }

    //----------------------------------------------

    private void Add(CommunityTypes communityType){
        communities.Add(communityType, new Community(communityType));
    }
}
