using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommunityManager
{
    private Dictionary<ChimeInputs, Community> communities = new Dictionary<ChimeInputs, Community>();

    //-------------------------------------------

    public CommunityManager(){
        int agentTypeCount = ChimeInputs.GetValues(typeof(ChimeInputs)).Length;

        for (int i = 0; i < agentTypeCount; i++){
            Add((ChimeInputs)i, new Community());
        }
    }

    public Community Get(ChimeInputs agentType){
        communities.TryGetValue(agentType, out Community community);

        #if UNITY_EDITOR
            if (communities == null) { Debug.LogError("Tried to get community but " + agentType.ToString() + " is not found"); }
        #endif
        
        return community;
    }

    //----------------------------------------------

    private void Add(ChimeInputs agentType, Community newCommunity){
        communities.Add(agentType, newCommunity);
    }
}
