using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommunityManager
{
    private Dictionary<AgentType, Community> communities = new Dictionary<AgentType, Community>();

    //-------------------------------------------

    public CommunityManager(){
        int agentTypeCount = AgentType.GetValues(typeof(AgentType)).Length;

        for (int i = 0; i < agentTypeCount; i++){
            Add((AgentType)i, new Community());
        }
    }

    public Community Get(AgentType agentType){
        communities.TryGetValue(agentType, out Community community);

        #if UNITY_EDITOR
            if (communities == null) { Debug.LogError("Tried to get community but " + agentType.ToString() + " is not found"); }
        #endif
        
        return community;
    }

    //----------------------------------------------

    private void Add(AgentType agentType, Community newCommunity){
        communities.Add(agentType, newCommunity);
    }
}
