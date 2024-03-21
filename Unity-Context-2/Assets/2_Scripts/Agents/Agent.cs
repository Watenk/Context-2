using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Agent : IFixedUpdateable
{
    public GameObject GameObject { get; private set; }
    public Group Group { get; private set; }
    public bool DestinationReached { get; private set; }
    public NavMeshAgent NavMeshAgent { get; private set; }
    public Fsm<Agent> fsm { get; private set; }

    // Pathfinding
    private List<Vector3> path = new List<Vector3>();
    private int pathIndex;
    private float wanderFromHomeDistance;
    private float stepDistance;
    private float stopDistance;

    // References
    private ChimeSequencer chimeSequencer;
    private SoundManager soundManager;

    //----------------------------------------

    public Agent(GameObject gameObject, Group group){
        GameObject = gameObject;
        Group = group;
        NavMeshAgent = GameManager.GetComponent<NavMeshAgent>(GameObject);
        chimeSequencer = GameManager.GetService<ChimeSequencer>();
        soundManager = GameManager.GetService<SoundManager>();
        stepDistance = AgentSettings.Instance.StepDistance;
        wanderFromHomeDistance = AgentSettings.Instance.WanderFromHomeDistance;
        NavMeshAgent.speed = Random.Range(AgentSettings.Instance.MinSpeed, AgentSettings.Instance.MaxSpeed);
        DestinationReached = true;

        chimeSequencer.OnChimeSequence += OnChimeSequence;

        fsm = new Fsm<Agent>(this,
           new AgentIdleState(),
           new AgentFollowingState(),
           new AgentWanderingState(),
           new AgentLookAtPlayerState(),
           new AgentDepressedState()
        );
        fsm.SwitchState(typeof(AgentDepressedState));

        #if UNITY_EDITOR
            if (NavMeshAgent == null) { Debug.LogError(gameObject.name + " doesn't contain a navmeshAgent"); }
            if (AgentSettings.Instance.MaxSpeed == 0) { Debug.LogError("MaxSpeed is 0 in AgentSettings"); }
            if (AgentSettings.Instance.MinSpeed == 0) { Debug.LogError("MinSpeed is 0 in AgentSettings"); }
            if (stepDistance == 0) { Debug.LogError("StepDistance is 0 in AgentSettings"); }
            if (wanderFromHomeDistance == 0) { Debug.LogError("WanderFromHome is 0 in AgentSettings"); }
        #endif
    }

    public void OnFixedUpdate(){
        fsm.OnUpdate();
        UpdatePath();
    }

    public void SetDestination(Vector3 pos, float stopDistance){
        this.stopDistance = stopDistance;
        NavMeshPath navMeshPath = new NavMeshPath();
        NavMeshAgent.CalculatePath(pos, navMeshPath);

        List<Vector3> path = new List<Vector3>();
        for (int i = 0; i < navMeshPath.corners.Length; i++){
            path.Add(navMeshPath.corners[i]);
        }

        path = DividePathIntoSteps(path);
        // path = GravitateTowardsSimilarAgents(path);
        // path = RemovePointsOutsideNavMesh(path);

        pathIndex = 0;
        this.path = path;
        DestinationReached = false;
    }

    //------------------------------------------

    private void OnChimeSequence(ChimeSequence chimeSequence){
        
        if (!chimeSequence.affectedCommunities.Contains(Group.CommunityType)) { return; } // If community is affected

        ExecuteTask(chimeSequence.chimeTask);
    }

    private void ExecuteTask(ChimeTasks chimeTask){

        switch (chimeTask){
            case ChimeTasks.follow:
                if (fsm.currentState == fsm.GetState(typeof(AgentLookAtPlayerState))){
                    fsm.SwitchState(typeof(AgentFollowingState));

                    // Sound
                    NPCSoundData soundData = soundManager.GetNPCSound(chimeTask);
                    soundManager.PlaySound(soundData, GameObject.transform.position);
                }
                break;
            
            case ChimeTasks.solveProblem:
                if (fsm.currentState == fsm.GetState(typeof(AgentFollowingState))){

                    // Sound
                    NPCSoundData soundData = soundManager.GetNPCSound(ChimeTasks.follow);
                    soundManager.PlaySound(soundData, GameObject.transform.position);
                }
                break;
        }
    }

    private void UpdatePath(){
        if (pathIndex != path.Count){
            if (NavMeshAgent.remainingDistance <= stopDistance){
                NavMeshAgent.SetDestination(path[pathIndex]);
                pathIndex++;
            }
        }
        else{
            DestinationReached = true;
        }
    }

    private List<Vector3> DividePathIntoSteps(List<Vector3> currentPath){
        
        List<Vector3> newPath = new List<Vector3>();
        Vector3 previousPos = NavMeshAgent.transform.position;

        for (int i = 0; i < currentPath.Count; i++){
            Vector3 currentPos = currentPath[i];

            // If distance between previous and current pos is too large
            float distance = Vector3.Distance(previousPos, currentPos);
            if (distance > stepDistance){
                int newStepsAmount = Mathf.CeilToInt(distance / stepDistance);

                for (int j = 0; j < newStepsAmount; j++){
                    float t = (float)j / (newStepsAmount + 1);
                    Vector3 newStep = Vector3.Lerp(previousPos, currentPos, t);
                    newPath.Add(newStep);
                }
            }
            else{
                newPath.Add(currentPos);
            }
            previousPos = currentPos;
        }
        return newPath;
    }

    // private List<Vector3> GravitateTowardsSimilarAgents(List<Vector3> currentPath){
    //     return currentPath;
    // }

    // private List<Vector3> RemovePointsOutsideNavMesh(List<Vector3> currentPath){
    //     // navMesh.SamplePosition to check if point is on navmesh
    //     return currentPath;
    // }
}
