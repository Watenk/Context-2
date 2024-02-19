using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Agent : MonoBehaviour
{
    public Group Group { get; private set; }

    // Pathfinding
    private NavMeshAgent agent;
    private List<Vector3> path = new List<Vector3>();
    private int pathIndex;
    private bool destinationReached;
    private float stepDistance;

    // References
    private GroupManager groupManager;

    //----------------------------------------

    public void Start(){
        agent = GetComponent<NavMeshAgent>();
        groupManager = GameManager.Instance.GetService<GroupManager>();
        stepDistance = AgentSettings.Instance.StepDistance;
        agent.speed = Random.Range(AgentSettings.Instance.MinSpeed, AgentSettings.Instance.MaxSpeed);

        #if UNITY_EDITOR
            if (agent == null) { Debug.LogError(gameObject.name + " doesn't contain a navmeshAgent"); }
            if (AgentSettings.Instance.MaxSpeed == 0) { Debug.LogError("MaxSpeed isn't set in AgentSettings"); }
            if (AgentSettings.Instance.MinSpeed == 0) { Debug.LogError("MinSpeed isn't set in AgentSettings"); }
            if (stepDistance == 0) { Debug.LogError("StepDistance isn't set in AgentSettings"); }
        #endif

        WalkTo(new Vector3(20, 0, 10));
    }

    public void FixedUpdate(){
        if (!destinationReached){

            float stopDistance;
            if (pathIndex == path.Count){
                stopDistance = 0.01f;
            }
            else{
                stopDistance = 1f;
            }

            if (agent.remainingDistance <= stopDistance){

                if (pathIndex == path.Count){
                    destinationReached = true;
                }
                else{
                    agent.SetDestination(path[pathIndex]);
                    pathIndex++;
                }
            }
        }
    }

    public void WalkTo(Vector3 pos){
        path = CalcPathTo(pos);
        pathIndex = 0;
        destinationReached = false;
    }

    //------------------------------------------

    private List<Vector3> CalcPathTo(Vector3 pos){
        NavMeshPath navMeshPath = new NavMeshPath();
        agent.CalculatePath(pos, navMeshPath);

        List<Vector3> path = new List<Vector3>();
        foreach (Vector3 current in navMeshPath.corners) { path.Add(current); }

        path = DividePathIntoSteps(path);
        path = GravitateTowardsSimilarAgents(path);
        path = RemovePointsOutsideNavMesh(path);

        return path;
    }

    private List<Vector3> DividePathIntoSteps(List<Vector3> currentPath){
        
        List<Vector3> newPath = new List<Vector3>();
        Vector3 previousPos = agent.transform.position;

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
