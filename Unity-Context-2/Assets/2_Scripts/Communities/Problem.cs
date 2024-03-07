using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Problem
{
    private List<CommunityTypes> problemSolvers;
    private Vector3 pos;
    private float detectRange;

    // References
    private PlayerController player;
    private CommunityManager communityManager;
    private ChimeSequencer chimeSequencer;

    //-------------------------------------------------

    public Problem(List<CommunityTypes> problemSolvers, Vector3 pos){
        this.problemSolvers = problemSolvers;
        this.pos = pos;

        communityManager = GameManager.GetService<CommunityManager>();
        chimeSequencer = GameManager.GetService<ChimeSequencer>();
        player = GameManager.Instance.Player;
        detectRange = ProblemSettings.Instance.ProblemDetectRange;

        chimeSequencer.OnChimeSequence += OnChimeSequence;

        #if UNITY_EDITOR
            if (detectRange == 0) { Debug.LogWarning("ProblemDetectRange in ProblemSettings is 0"); }
        #endif
    }

    //--------------------------------------------------

    private void OnChimeSequence(ChimeSequence chimeSequence){
        
        if (chimeSequence.chimeTask != ChimeTasks.solveProblem ) { return; }

        if (Vector3.Distance(pos, player.gameObject.transform.position) < detectRange){
            List<CommunityTypes> followingAgents = communityManager.GetFollowingAgents();

            if (problemSolvers.All(agent => followingAgents.Contains(agent))){
                Debug.Log("Jeee");
            }
        }
    }
}
