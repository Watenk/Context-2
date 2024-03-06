using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Problem : IFixedUpdateable
{
    private List<CommunityTypes> problemSolvers;
    private Vector3 pos;
    private float detectRange;
    private Timer detectTimer;

    // References
    private PlayerController player;
    private TimerManager timerManager;
    private CommunityManager communityManager;

    //-------------------------------------------------

    public Problem(List<CommunityTypes> problemSolvers, Vector3 pos){
        this.problemSolvers = problemSolvers;
        this.pos = pos;

        timerManager = GameManager.GetService<TimerManager>();
        communityManager = GameManager.GetService<CommunityManager>();
        player = GameManager.Instance.Player;
        detectTimer = timerManager.AddTimer(5);
        detectRange = ProblemSettings.Instance.ProblemDetectRange;

        #if UNITY_EDITOR
            if (detectRange == 0) { Debug.LogWarning("ProblemDetectRange in ProblemSettings is 0"); }
        #endif
    }

    public void OnFixedUpdate(){
        
        CheckIfProblemSolversAreInRange();
    }

    //--------------------------------------------------

    private void CheckIfProblemSolversAreInRange(){
        
        // TODO: Add DetectTimer

        if (Vector3.Distance(pos, player.gameObject.transform.position) < detectRange){
            List<CommunityTypes> followingAgents = communityManager.GetFollowingAgents();

            if (problemSolvers.All(agent => followingAgents.Contains(agent))){
                Debug.Log("Jeee");
            }
        }
    }
}
