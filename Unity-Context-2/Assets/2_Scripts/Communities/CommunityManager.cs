using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommunityManager : IFixedUpdateable
{
    public Action<CommunityTypes> OnFollow;

    private Dictionary<CommunityTypes, int> activeAgents = new Dictionary<CommunityTypes, int>();
    private Dictionary<CommunityTypes, Community> communities = new Dictionary<CommunityTypes, Community>();
    private Dictionary<CommunityTypes, AgentSpeedData> communitySpeedsData = new Dictionary<CommunityTypes, AgentSpeedData>();
    private Dictionary<CommunityTypes, float> communitySpeeds = new Dictionary<CommunityTypes, float>();
    private Dictionary<CommunityTypes, int> communityProblemsSolved = new Dictionary<CommunityTypes, int>();

    //-------------------------------------------

    public CommunityManager(){

        foreach (AgentSpeedData current in AgentSettings.Instance.agentsSpeeds){
            communitySpeeds.Add(current.communityType, current.initialSpeed);
        }

        foreach (AgentSpeedData current in AgentSettings.Instance.agentsSpeeds){
            communitySpeedsData.Add(current.communityType, current);
        }

        foreach (AgentSpeedData current in AgentSettings.Instance.agentsSpeeds){
            communityProblemsSolved.Add(current.communityType, 0);
        }

        Add(CommunityTypes.circle);
        Add(CommunityTypes.triangle);
        Add(CommunityTypes.square);
        Add(CommunityTypes.global);

        EventManager.AddListener<CommunityTypes>(Events.OnProblemSolved, (communityType) => RecalcSpeed(communityType));
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

    public Group AddGroup(CommunityTypes communityType, int size, float wanderFromHomeDistance, Vector3 pos, float spawnRadius, bool isActive){
        return GetCommunity(communityType).AddGroup(size, pos, wanderFromHomeDistance, spawnRadius, isActive);
    }

    public void AddProblem(CommunityTypes communityType, Problem problem){
        GetCommunity(communityType).AddProblem(problem);
    }

    public void RemoveProblem(CommunityTypes communityType, Problem problem){
        GetCommunity(communityType).RemoveProblem(problem);

        AkSoundEngine.SetState(2447030866U, 788884573U);

        switch (communityType){

            case CommunityTypes.square:
                AkSoundEngine.SetState(167474714U, 930712164U);
                break;

            case CommunityTypes.circle:
                AkSoundEngine.SetState(1316677579U, 930712164U);
                break;

            case CommunityTypes.triangle:
                AkSoundEngine.SetState(765250607U, 930712164U);
                break;
        }

        foreach (KeyValuePair<CommunityTypes, Community> kvp in communities){
            kvp.Value.ProblemSolved();
        }
    }

    public float GetSpeed(CommunityTypes communityType){
        communitySpeeds.TryGetValue(communityType, out float speed);
        return speed;
    }

    //----------------------------------------------

    private void RecalcSpeed(CommunityTypes communityType){
        communityProblemsSolved[communityType] += 1;
        AgentSpeedData agentSpeedData = communitySpeedsData[communityType];
        if (communityProblemsSolved[communityType] >= 3){
            communitySpeeds[communityType] = agentSpeedData.problemsSolvedSpeed;
        }
        else{
            float speedDifference = (agentSpeedData.problemsSolvedSpeed - agentSpeedData.initialSpeed) / 3;
            communitySpeeds[communityType] = agentSpeedData.initialSpeed + (speedDifference * communityProblemsSolved[communityType]);
        }

        Debug.Log("Recalc CommunitySpeed");

        EventManager.Invoke(Events.OnCommunitySpeedChange, communityType);
    }

    private void Add(CommunityTypes communityType){
        Community newCommunity = new Community(communityType);
        newCommunity.OnFollow += Follow;
        communities.Add(communityType, newCommunity);
        activeAgents.Add(communityType, 0);
    }

    private void Follow(CommunityTypes communityType){
        OnFollow?.Invoke(communityType);

        AkSoundEngine.SetState(2447030866U, 1256202815U);

        switch (communityType){

            case CommunityTypes.square:
                AkSoundEngine.SetState(167474714U, 1651971902U);
                break;

            case CommunityTypes.circle:
                AkSoundEngine.SetState(1316677579U, 1651971902U);
                break;

            case CommunityTypes.triangle:
                AkSoundEngine.SetState(765250607U, 1651971902U);
                break;
        }
    }
}
