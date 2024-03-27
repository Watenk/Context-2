using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Group : IFixedUpdateable
{
    public Action<CommunityTypes> OnFollow;
    public Community Community { get; private set; }
    public CommunityTypes CommunityType { get; private set; }
    public int Size { get; private set; }
    public Vector3 Home { get; private set; }
    public float SpawnRadius { get; private set; }
    public float WanderFromHomeDistance { get; private set; }

    private List<Agent> agents = new List<Agent>();
    private List<AgentPrefab> agentPrefabs = new List<AgentPrefab>();

    //-----------------------------------------------

    public Group(Community community, CommunityTypes communityType, int groupSize, float wanderFromHomeDistance, Vector3 homePos, float spawnRadius, bool isActive){
        Community = community;
        CommunityType = communityType;
        WanderFromHomeDistance = wanderFromHomeDistance;
        Size = groupSize;
        Home = homePos;
        SpawnRadius = spawnRadius;
        agentPrefabs = AgentSettings.Instance.Prefabs;

        InstanceAgents(isActive);
    }

    public void OnFixedUpdate(){
        foreach (Agent currentAgent in agents){
            currentAgent.OnFixedUpdate();
        }
    }

    public void ProblemSolved(){
        foreach (Agent current in agents){
            if (current.fsm.currentState == current.fsm.GetState(typeof(AgentFollowingState))){
                current.fsm.SwitchState(typeof(AgentWanderingState));
            }
        }
    }

    public void DestroyAgents(){
        foreach (Agent current in agents){
            GameObject.Destroy(current.GameObject);
        }
        EventManager.Invoke(Events.OnProblemSolved, CommunityType);
    }

    public void FreeAgents(){

        foreach (Agent current in agents){
            if (current.fsm.currentState == current.fsm.GetState(typeof(AgentDepressedState))){
                current.fsm.SwitchState(typeof(AgentWanderingState));
            }
        }
    }

    //------------------------------------------------

    private void Follow(CommunityTypes communityType){
        OnFollow(communityType);
    }

    private void InstanceAgents(bool isActive){
        
        AgentPrefab agentPrefab = GetAgentPrefab();

        for (int i = 0; i < Size; i++){

            // Instance GameObject
            GameObject randomPrefab = agentPrefab.prefabs[UnityEngine.Random.Range(0, agentPrefab.prefabs.Count)];
            Vector3 randomPosInRange = new Vector3(Home.x + UnityEngine.Random.Range(-SpawnRadius, SpawnRadius), 0, Home.z + UnityEngine.Random.Range(-SpawnRadius, SpawnRadius));
            GameObject agentInstance = GameObject.Instantiate(randomPrefab, randomPosInRange, Quaternion.identity);
            agentInstance.transform.SetParent(GameManager.Instance.transform);
            Animator animator = agentInstance.GetComponentInChildren<Animator>();
            ParticleSystem[] particleSystems = agentInstance.GetComponentsInChildren<ParticleSystem>();
            BubbleController bubbleController = agentInstance.GetComponentInChildren<BubbleController>();

            #if UNITY_EDITOR
            if (animator == null) { Debug.LogError(randomPrefab.name + " Doesn't contain a Animator"); }
            if (particleSystems.Length < 2) { Debug.LogError(randomPrefab.name + " Is missing a particle System"); }
            if (bubbleController == null) { Debug.LogError(randomPrefab.name + " Doesn't contain a Bubble Controller "); }
#endif

            Agent newAgent = new Agent(agentInstance, this, animator, particleSystems, bubbleController);
            if (isActive) { newAgent.fsm.SwitchState(typeof(AgentWanderingState)); }
            newAgent.OnFollow += Follow;
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
            if (agentPrefab == null) { Debug.LogError("No AgentPrefab of type " + Community.CommunityType.ToString() + " found. Add this type in the AgentSettings"); }
            if (agentPrefab.prefabs == null) { Debug.LogError("No prefabs of type " + Community.ToString() + " found. Add prefabs of this type in the AgentSettings"); }
        #endif

        return agentPrefab;
    }
}
