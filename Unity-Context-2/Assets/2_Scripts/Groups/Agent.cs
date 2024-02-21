using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Agent : MonoBehaviour
{
    public Group Group { get; private set; }
    public NavMeshAgent NavMeshAgent { get; private set; }
    public bool DestinationReached { get; private set; }

    // Pathfinding
    private List<Vector3> path = new List<Vector3>();
    private int pathIndex;
    private float stepDistance;
    private float wanderFromHomeDistance;

    // References
    private GroupsManager groupManager;

    //----------------------------------------

    public void InitAgent(Group group){
        Group = group;
        NavMeshAgent = GetComponent<NavMeshAgent>();
        groupManager = GameManager.Instance.GetService<GroupsManager>();
        stepDistance = AgentSettings.Instance.StepDistance;
        wanderFromHomeDistance = AgentSettings.Instance.WanderFromHomeDistance;
        NavMeshAgent.speed = Random.Range(AgentSettings.Instance.MinSpeed, AgentSettings.Instance.MaxSpeed);
        DestinationReached = true;

        #if UNITY_EDITOR
            if (NavMeshAgent == null) { Debug.LogError(gameObject.name + " doesn't contain a navmeshAgent"); }
            if (AgentSettings.Instance.MaxSpeed == 0) { Debug.LogError("MaxSpeed is 0 in AgentSettings"); }
            if (AgentSettings.Instance.MinSpeed == 0) { Debug.LogError("MinSpeed is 0 in AgentSettings"); }
            if (stepDistance == 0) { Debug.LogError("StepDistance is 0 in AgentSettings"); }
            if (wanderFromHomeDistance == 0) { Debug.LogError("WanderFromHome is 0 in AgentSettings"); }
        #endif
    }

    public void FixedUpdate(){
        
        if (DestinationReached) { GetNewDestination(); }
        else { FollowPath(); }
    }

    //------------------------------------------

    private void FollowPath(){
        if (NavMeshAgent.remainingDistance <= 1.0f){
            if (pathIndex == path.Count){
                DestinationReached = true;
            }
            else{
                NavMeshAgent.SetDestination(path[pathIndex]);
                pathIndex++;
            }
        }
    }

    private void GetNewDestination(){
        Vector3 newDestination = new Vector3(Group.Home.x + Random.Range(-wanderFromHomeDistance, wanderFromHomeDistance), 0, Group.Home.z + Random.Range(-wanderFromHomeDistance, wanderFromHomeDistance));
        path = CalcPathTo(newDestination);
        pathIndex = 0;
        DestinationReached = false;
    }

    private List<Vector3> CalcPathTo(Vector3 pos){
        NavMeshPath navMeshPath = new NavMeshPath();
        NavMeshAgent.CalculatePath(pos, navMeshPath);

        List<Vector3> path = new List<Vector3>();
        foreach (Vector3 current in navMeshPath.corners) { path.Add(current); }

        path = DividePathIntoSteps(path);
        path = GravitateTowardsSimilarAgents(path);
        path = RemovePointsOutsideNavMesh(path);

        return path;
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

    private List<Vector3> GravitateTowardsSimilarAgents(List<Vector3> currentPath){
        return currentPath;
    }

    private List<Vector3> RemovePointsOutsideNavMesh(List<Vector3> currentPath){
        // navMesh.SamplePosition to check if point is on navmesh
        return currentPath;
    }
}
