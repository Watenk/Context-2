using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Community
{
    private Communities community;
    private List<Group> groups = new List<Group>();
    private List<Problem> problems = new List<Problem>();
    // TODO: Add Affection for other communities

    // References
    private ChimeSequencer chimeSequencer;

    //------------------------------------------------

    public Community(Communities community){
        chimeSequencer = GameManager.GetService<ChimeSequencer>();
        this.community = community;

        chimeSequencer.OnChimeSequence += OnChimeSequence;
    }

    //--------------------------------------------------

    private void OnChimeSequence(ChimeSequence chimeSequence){
        if (chimeSequence.affectedCommunities.Contains(community)){
            foreach (Group currentGroup in groups){
                currentGroup.ExecuteTask(chimeSequence.chimeTask);
            }
        }
    }
}
