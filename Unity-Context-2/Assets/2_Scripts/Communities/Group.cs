using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Group : IFixedUpdateable
{
    public Community Community { get; private set; }
    public CommunityTypes CommunityType { get; private set; }
    public int Size { get; private set; }
    public Vector3 Home { get; private set; }
    public float SpawnRadius { get; private set; }

    private List<Agent> agents = new List<Agent>();
    private List<AgentPrefab> agentPrefabs = new List<AgentPrefab>();

    //-----------------------------------------------

    public Group(Community community, CommunityTypes communityType, int groupSize, Vector3 homePos, float spawnRadius){
        Community = community;
        CommunityType = communityType;
        Size = groupSize;
        Home = homePos;
        SpawnRadius = spawnRadius;
        agentPrefabs = AgentSettings.Instance.Prefabs;

        InstanceAgents();
    }

    public void OnFixedUpdate(){
        foreach (Agent currentAgent in agents){
            currentAgent.OnFixedUpdate();
        }
    }

    public void DestroyAgents(){
        foreach (Agent current in agents){
            GameObject.Destroy(current.GameObject);
        }
    }

    //------------------------------------------------

    private void InstanceAgents(){
        
        AgentPrefab agentPrefab = GetAgentPrefab();

        for (int i = 0; i < Size; i++){

            // Instance GameObject
            GameObject randomPrefab = agentPrefab.prefabs[Random.Range(0, agentPrefab.prefabs.Count)];
            Vector3 randomPosInRange = new Vector3(Home.x + Random.Range(-SpawnRadius, SpawnRadius), 0, Home.z + Random.Range(-SpawnRadius, SpawnRadius));
            GameObject agentInstance = GameObject.Instantiate(randomPrefab, randomPosInRange, Quaternion.identity);
            agentInstance.transform.SetParent(GameManager.Instance.transform);

            Agent newAgent = new Agent(agentInstance, this);
            agents.Add(newAgent);
        }
    }

    private AgentPrefab GetAgentPrefab(){

        AgentPrefab agentPrefab = null;
        foreach (AgentPrefab current in agentPrefabs){
            if (current.CommunityType == Community.CommunityType){
                agentPrefab = current;
            }
        }

        #if UNITY_EDITOR
            if (agentPrefab == null) { Debug.LogError("No AgentPrefab of type " + Community.ToString() + " found. Add this type in the AgentSettings"); }
            if (agentPrefab.prefabs == null) { Debug.LogError("No prefabs of type " + Community.ToString() + " found. Add prefabs of this type in the AgentSettings"); }
        #endif

        return agentPrefab;
    }
}
