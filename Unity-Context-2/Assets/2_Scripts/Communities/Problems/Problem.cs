using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Problem : IFixedUpdateable
{
    private Dictionary<CommunityTypes, List<ProblemSolver>> problemSolvers = new Dictionary<CommunityTypes, List<ProblemSolver>>();
    private List<MushroomPrefab> mushroomPrefabs;
    private List<CommunityTypes> communityAmount;
    private Vector3 pos;
    private float detectRange;
    private float mushroomSpawnRadius;
    private GameObject gameObject;

    // References
    private PlayerController player;
    private CommunityManager communityManager;
    private ChimeSequencer chimeSequencer;

    //-------------------------------------------------

    public Problem(List<CommunityTypes> communityTypes, GameObject gameObject, Vector3 pos){
        this.gameObject = gameObject;
        this.pos = pos;

        communityManager = GameManager.GetService<CommunityManager>();
        chimeSequencer = GameManager.GetService<ChimeSequencer>();
        player = GameManager.Instance.Player;
        detectRange = ProblemSettings.Instance.ProblemDetectRange;
        mushroomSpawnRadius = ProblemSettings.Instance.MushroomSpawnRadius;
        mushroomPrefabs = ProblemSettings.Instance.MushroomPrefabs;

        chimeSequencer.OnChimeSequence += OnChimeSequence;

        // Init problemSolvers dict
        communityAmount = communityManager.GetCommunityAmount();
        foreach (CommunityTypes currentCommunity in communityAmount){
            problemSolvers.Add(currentCommunity, new List<ProblemSolver>());
        }

        SpawnMushrooms(communityTypes);

        #if UNITY_EDITOR
            if (detectRange == 0) { Debug.LogWarning("ProblemDetectRange in ProblemSettings is 0"); }
            if (mushroomSpawnRadius == 0) { Debug.LogWarning("mushroomSpawnRadius in ProblemSettings is 0"); }
        #endif
    }

    public void OnFixedUpdate(){

        if (Vector3.Distance(pos, player.gameObject.transform.position) > detectRange) { return; }

        foreach (CommunityTypes currentCommunity in communityAmount){
            List<ProblemSolver> solvers = problemSolvers[currentCommunity];
            int followingAmount = GetFollowingAmount(currentCommunity);

            foreach (ProblemSolver currentSolver in solvers){
                if (followingAmount > 0){
                    currentSolver.Animator.SetBool("Active", true);
                    followingAmount -= 1;
                }
                else{
                    currentSolver.Animator.SetBool("Active", false);
                }
            }
        }
    }

    //--------------------------------------------------

    private void OnChimeSequence(ChimeSequence chimeSequence){
        
        if (chimeSequence.chimeTask != ChimeTasks.solveProblem ) { return; }
        if (Vector3.Distance(pos, player.gameObject.transform.position) > detectRange) { return; }

        // Dictionary<CommunityTypes, int> followingAgents = communityManager.GetActiveAgents();
        // if (problemSolvers.All(problemSolver => followingAgents.Contains(problemSolver.CommunityType))){
        //     Debug.Log("Problem Solved");
        // }
    }

    private void SpawnMushrooms(List<CommunityTypes> communityTypes){
        for (int i = 0; i < communityTypes.Count; i++)
        {
            CommunityTypes communityType = communityTypes[i];
            float mushroomAngle = i * Mathf.PI * 2 / communityTypes.Count;
            Vector3 mushroomPos = new Vector3(Mathf.Cos(mushroomAngle), 0, Mathf.Sin(mushroomAngle)) * mushroomSpawnRadius + gameObject.transform.position;
            GameObject newMushroom = GameObject.Instantiate(mushroomPrefabs.Find(problemPrefab => problemPrefab.communityType == communityType).gameObject, mushroomPos, Quaternion.identity);
            newMushroom.transform.SetParent(gameObject.transform);
            problemSolvers[communityType].Add(new ProblemSolver(communityType, newMushroom));
        }
    }

    private int GetFollowingAmount(CommunityTypes communityType){

        Dictionary<CommunityTypes, int> followingAgents = communityManager.GetActiveAgents();   
        return followingAgents[communityType];
    }
}
