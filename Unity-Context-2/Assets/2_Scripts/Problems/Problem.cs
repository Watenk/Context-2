using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using UnityEngine;

public class Problem : IFixedUpdateable
{
    public int FreeNpcAmount { get; private set; }

    private Dictionary<CommunityTypes, List<ProblemSolver>> problemSolvers = new Dictionary<CommunityTypes, List<ProblemSolver>>();
    private Dictionary<CommunityTypes, bool> solvedCommunities = new Dictionary<CommunityTypes, bool>();
    private List<ProblemMushroomPrefab> mushroomPrefabs;
    private List<CommunityTypes> communityAmount;
    private CommunityTypes communityType;
    private Vector3 pos;
    private float detectRange;
    private float mushroomSpawnRadius;
    private GameObject gameObject;

    // References
    private PlayerController player;
    private CommunityManager communityManager;
    private ChimeSequencer chimeSequencer;

    //-------------------------------------------------

    public Problem(List<CommunityTypes> solverCommunityTypes, CommunityTypes communityType, int freeNpcAmount, GameObject gameObject, Vector3 pos){
        this.communityType = communityType;
        this.FreeNpcAmount = freeNpcAmount;
        this.gameObject = gameObject;
        this.pos = pos;

        communityManager = GameManager.GetService<CommunityManager>();
        chimeSequencer = GameManager.GetService<ChimeSequencer>();
        player = GameManager.Instance.Player;
        detectRange = ProblemSettings.Instance.ProblemDetectRange;
        mushroomSpawnRadius = ProblemSettings.Instance.MushroomSpawnRadius;
        mushroomPrefabs = ProblemSettings.Instance.MushroomPrefabs;

        chimeSequencer.OnChimeSequence += OnChimeSequence;

        // Init Dicts
        communityAmount = communityManager.GetCommunityAmount();
        foreach (CommunityTypes currentCommunity in communityAmount){
            problemSolvers.Add(currentCommunity, new List<ProblemSolver>());
        }
        foreach (CommunityTypes currentCommunity in solverCommunityTypes){
            if (!solvedCommunities.ContainsKey(currentCommunity)){
                solvedCommunities.Add(currentCommunity, false);
            }
        }

        SpawnMushrooms(solverCommunityTypes);

        #if UNITY_EDITOR
            if (detectRange == 0) { Debug.LogWarning("ProblemDetectRange in ProblemSettings is 0"); }
            if (mushroomSpawnRadius == 0) { Debug.LogWarning("mushroomSpawnRadius in ProblemSettings is 0"); }
        #endif
    }

    public void OnFixedUpdate(){

        if (Vector3.Distance(pos, player.gameObject.transform.position) > detectRange) { return; }

        // Set mushroom animations
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

    public void Remove(){
        chimeSequencer.OnChimeSequence -= OnChimeSequence;
        
        foreach (KeyValuePair<CommunityTypes, List<ProblemSolver>> kvp in problemSolvers){
            List<ProblemSolver> currentProblemSolvers = kvp.Value;

            foreach (ProblemSolver problemSolver in currentProblemSolvers){
                GameObject.Destroy(problemSolver.Mushroom);
            }
        }
        GameObject.Destroy(gameObject);
    }

    //--------------------------------------------------

    private void OnChimeSequence(ChimeSequence chimeSequence){
        
        if (chimeSequence.chimeTask != ChimeTasks.solveProblem ) { return; }
        if (Vector3.Distance(pos, player.gameObject.transform.position) > detectRange) { return; }

        // Check if enough followingAgents are present
        foreach (CommunityTypes currentCommunity in communityAmount){
            List<ProblemSolver> solvers = problemSolvers[currentCommunity];
            int followingAmount = GetFollowingAmount(currentCommunity);

            foreach (ProblemSolver currentSolver in solvers){
                if (followingAmount > 0){
                    followingAmount -= 1;
                }
                else{
                    return;
                }
            }
        }

        // Set solved communities
        foreach (CommunityTypes currentType in chimeSequence.affectedCommunities){
            solvedCommunities[currentType] = true;
        }

        // Check if all communities are solved
        foreach (KeyValuePair<CommunityTypes, bool> kvp in solvedCommunities){
            if (!kvp.Value) { return; }
        }

        communityManager.RemoveProblem(communityType, this);
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
