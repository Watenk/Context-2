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

    [Header("Prefabs")]
    public List<AgentPrefab> Prefabs = new List<AgentPrefab>();

    [Header("Settings")]
    public float MaxSpeed;
    public float MinSpeed;
    [Tooltip("Distance an agent will wander from their home")]
    public float WanderFromHomeDistance;

    [Header("Advanced Settings")]
    [Tooltip("Max Distance between each 'step' in the agents path")]
    public float StepDistance; 
}
