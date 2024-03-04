using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Community
{
    private List<Group> groups = new List<Group>();
    private List<Problem> problems = new List<Problem>();
    private List<ChimeSequences> chimeSequences = new List<ChimeSequences>();

    // References
    private ChimeSequencer chimeSequencer;

    //------------------------------------------------

    public Community(){
        chimeSequencer = GameManager.GetService<ChimeSequencer>();

        chimeSequencer.OnChimeSequence += OnChimeSequence;
    }

    //--------------------------------------------------

    private void OnChimeSequence(ChimeSequences chimeSequence){
        
    }
}
