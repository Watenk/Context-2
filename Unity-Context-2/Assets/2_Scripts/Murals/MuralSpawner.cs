using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuralSpawner : MonoBehaviour
{
    public ChimeSequence chimeSequence;

    //-------------------------------------

    public void Start(){
        GameManager.GetService<MuralManager>().Add(new Mural(chimeSequence, this.gameObject));
        Destroy(this);
    }
}
