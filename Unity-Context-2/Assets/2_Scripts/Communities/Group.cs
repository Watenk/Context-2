using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class Group
{
    public ChimeInputs AgentType { get; private set; }
    public int Size { get; private set; }
    public Vector3 Home { get; private set; }
    public float SpawnRadius { get; private set; }

    private List<Agent> agents = new List<Agent>();
    private List<AgentPrefab> agentPrefabs = new List<AgentPrefab>();

    //-----------------------------------------------

    public Group(ChimeInputs agentType, int groupSize, Vector3 homePos, float spawnRadius){
        AgentType = agentType;
        Size = groupSize;
        Home = homePos;
        SpawnRadius = spawnRadius;
        agentPrefabs = AgentSettings.Instance.Prefabs;

        InstanceAgents();
    }

    public void DestroyAgents(){
        foreach (Agent current in agents){
            GameObject.Destroy(current.GameObject);
        }
    }

    public void ExecuteTask(ChimeTasks chimeTask){
        foreach (Agent currentAgent in agents){
            currentAgent.ExecuteTask(chimeTask);
        }
    }

    //------------------------------------------------

    private void InstanceAgents(){
        
        AgentPrefab agentPrefab = GetAgentPrefab();

        for (int i = 0; i < Size; i++){

            GameObject randomPrefab = agentPrefab.prefabs[Random.Range(0, agentPrefab.prefabs.Count)];
            Vector3 randomPosInRange = new Vector3(Home.x + Random.Range(-SpawnRadius, SpawnRadius), 0, Home.z + Random.Range(-SpawnRadius, SpawnRadius));
            GameObject agentInstance = GameObject.Instantiate(randomPrefab, randomPosInRange, Quaternion.identity);
            agentInstance.transform.SetParent(GameManager.Instance.transform);
            Agent agent = agentInstance.GetComponent<Agent>();

            #if UNITY_EDITOR
                if (agent == null) { Debug.LogError(agentPrefab.ToString() + " doesn't contain an agent"); }
            #endif

            agents.Add(agent);
        }
    }

    private AgentPrefab GetAgentPrefab(){

        AgentPrefab agentPrefab = null;
        foreach (AgentPrefab current in agentPrefabs){
            if (current.agentType == AgentType){
                agentPrefab = current;
            }
        }

        #if UNITY_EDITOR
            if (agentPrefab == null) { Debug.LogError("No AgentPrefab of type " + AgentType.ToString() + " found. Add this type in the AgentSettings"); }
            if (agentPrefab.prefabs == null) { Debug.LogError("No prefabs of type " + AgentType.ToString() + " found. Add prefabs of this type in the AgentSettings"); }
        #endif

        return agentPrefab;
    }
}
