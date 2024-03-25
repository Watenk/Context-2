using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuralManager : IFixedUpdateable
{
    private List<Mural> murals = new List<Mural>();
    private List<Mural> inactiveMurals = new List<Mural>();
    private List<Mural> inactiveMuralsGC = new List<Mural>();

    // References
    private ChimeSequencer chimeSequencer;

    //----------------------------------------------

    public MuralManager(){
        chimeSequencer = GameManager.GetService<ChimeSequencer>();

        chimeSequencer.OnChimeSequence += OnChimeSequence;
    }

    public void OnFixedUpdate(){
        foreach (Mural mural in murals){
            mural.OnFixedUpdate();
        }
    }

    public void AddMural(Mural mural){
        if (mural.LibraryMural) inactiveMurals.Add(mural);
        else murals.Add(mural);
    }

    public Mural GetMural(ChimeSequence chimeSequence){
        
        Mural mural = murals.Find(mural => mural.ChimeSequence == chimeSequence);

        if (mural == null) return null;
        else return mural;
    }

    public Mural GetMural(ChimeSequences chimeSequence){
        
        Mural mural = murals.Find(mural => mural.ChimeSequence.chimeSequence == chimeSequence);

        if (mural == null) return null;
        else return mural;
    }

    //-------------------------------------------------

    private void OnChimeSequence(ChimeSequence chimeSequence){

        foreach (Mural current in inactiveMurals){
            if (current.ChimeSequence == chimeSequence){
                inactiveMuralsGC.Add(current);
                current.SpawnMushrooms();
                murals.Add(current);
            }
        }
        inactiveMuralsGC.Clear();
    }
}
