using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuralSpawner : MonoBehaviour
{
    public ChimeSequences chimeSequence;

    //-------------------------------------

    public void Start(){
        List<ChimeSequence> chimeSequences = ChimeSettings.Instance.ChimeSequences;
        GameManager.GetService<MuralManager>().Add(new Mural(chimeSequences.Find(chime => chime.chimeSequence == chimeSequence), this.gameObject));
        Destroy(this);
    }
}
