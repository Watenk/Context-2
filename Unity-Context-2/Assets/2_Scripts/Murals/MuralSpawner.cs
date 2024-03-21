using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuralSpawner : MonoBehaviour
{
    public ChimeSequences chimeSequence;
    [Tooltip("If this is enabled the mural will only work after the selected chimeSequence has been performed once")]
    public bool LibraryMural;

    //-------------------------------------

    public void Start(){
        List<ChimeSequence> chimeSequences = ChimeSettings.Instance.ChimeSequences;
        GameManager.GetService<MuralManager>().AddMural(new Mural(chimeSequences.Find(chime => chime.chimeSequence == chimeSequence), this.gameObject, LibraryMural));
        Destroy(this);
    }
}
