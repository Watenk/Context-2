using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommunityManager : IFixedUpdateable
{
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

    public void AddGroup(CommunityTypes communityType, int size, Vector3 pos, float spawnRadius){
        Get(communityType).AddGroup(size, pos, spawnRadius);
    }

    public Community GetCommunity(CommunityTypes communityType){
        communities.TryGetValue(communityType, out Community community);

        #if UNITY_EDITOR
            if (community == null) { Debug.LogError("Tried to get " + communityType.ToString() + " from communities but its null"); }
        #endif

        return community;
    }

    public List<CommunityTypes> GetFollowingAgents(){

        if (communities.Count == 0) { return default; }
        List<CommunityTypes> followingAgents = new List<CommunityTypes>();

        foreach (KeyValuePair<CommunityTypes, Community> kvp in communities){
            followingAgents.AddRange(kvp.Value.GetFollowingAgents());
        }

        return followingAgents;
    }

    public void AddProblem(CommunityTypes communityType, Problem problem){
        Get(communityType).AddProblem(problem);
    }

    //----------------------------------------------

    private void Add(CommunityTypes communityType){
        communities.Add(communityType, new Community(communityType));
    }

    private Community Get(CommunityTypes communityType){
        communities.TryGetValue(communityType, out Community communityInstance);

        #if UNITY_EDITOR
            if (communityInstance == null) { Debug.LogWarning("communityInstance of " + communityType.ToString() + " is null"); }
        #endif

        return communityInstance;
    }
}
