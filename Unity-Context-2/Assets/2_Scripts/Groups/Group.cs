using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class Group
{
    public AgentType AgentType { get; private set; }
    public int Size { get; private set; }
    public Vector3 Home { get; private set; }
    public float SpawnRadius { get; private set; }

    private List<Agent> agents = new List<Agent>();
    private List<AgentPrefab> agentPrefabs = new List<AgentPrefab>();

    // References
    private GroupManager groupManager;

    //-----------------------------------------------

    public Group(AgentType agentType, int groupSize, Vector3 homePos, float spawnRadius, GroupManager groupManager){
        AgentType = agentType;
        Size = groupSize;
        Home = homePos;
        SpawnRadius = spawnRadius;
        this.groupManager = groupManager;
        agentPrefabs = AgentSettings.Instance.Prefabs;

        InstanceActors();
    }

    public void DestroyAgents(){
        foreach (Agent current in agents){
            GameObject.Destroy(current.gameObject);
        }
    }

    //------------------------------------------------

    private void InstanceActors(){
        
        AgentPrefab agentPrefab = GetAgentPrefab();

        for (int i = 0; i < Size; i++){

            GameObject randomPrefab = agentPrefab.prefabs[Random.Range(0, agentPrefab.prefabs.Count)];
            Vector3 randomPosInRange = new Vector3(Home.x + Random.Range(-SpawnRadius, SpawnRadius), 0, Home.z + Random.Range(-SpawnRadius, SpawnRadius));
            GameObject agentInstance = GameObject.Instantiate(randomPrefab, randomPosInRange, Quaternion.identity);
            agentInstance.transform.SetParent(GameManager.Instance.transform);
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
