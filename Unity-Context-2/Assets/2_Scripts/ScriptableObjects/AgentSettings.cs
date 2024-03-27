using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AgentSettings", menuName = "ScriptableObjects/AgentSettings")]
public class AgentSettings : ScriptableObject
{
    public static AgentSettings Instance { 
        get{
            if (instance == null){
                instance = Resources.Load<AgentSettings>("AgentSettings");

                #if UNITY_EDITOR
                    if (instance == null) {Debug.LogError("AgentSettings couldn't be loaded...");}
                #endif
            }
            return instance;
        }
    }
    private static AgentSettings instance;

    //------------------------------------------------------------------

    [Header("Settings")]
    public List<AgentSpeedData> agentsSpeeds = new List<AgentSpeedData>();

    [Tooltip("Distance an agent will wander from their home")]
    public float LookAtPlayerDistance;
    [Tooltip("The Distance an agent follows the player at")]
    public float FollowPlayerAtDistance;
    [Tooltip("The Speed an agent follows the player at")]
    public float FollowPlayerSpeed;
    public float WalkToLibChance;

    [Header("Prefabs")]
    public List<AgentPrefab> Prefabs = new List<AgentPrefab>();
}
