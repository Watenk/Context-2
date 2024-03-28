using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuralHint : MonoBehaviour
{
    public GameObject FollowHint;
    public GameObject SolveHit;

    private Timer hintTimer;
    private MuralManager muralManager;

    void Start(){
        hintTimer = GameManager.GetService<TimerManager>().AddTimer(3.0f);
        muralManager = GameManager.GetService<MuralManager>();

        for (int i = 0; i < Enum.GetNames(typeof(ChimeSequences)).Length; i++){
            Mural mural = muralManager.GetMural(((ChimeSequences)i));
            if (mural != null) mural.OnPlayer += OnMural;
        }
    }

    void Update(){
        if (hintTimer.IsDone()){
            FollowHint.SetActive(false);
            SolveHit.SetActive(false);
        }
    }

    private void OnMural(ChimeSequences chimeSequence){
        hintTimer.Reset();
        Debug.Log(chimeSequence.ToString());
        if (chimeSequence == ChimeSequences.TriangleFollow || chimeSequence == ChimeSequences.CrossFollow || chimeSequence == ChimeSequences.SquareFollow || chimeSequence == ChimeSequences.CircleFollow){
            FollowHint.SetActive(true);
        }

        if (chimeSequence == ChimeSequences.CrossSolve || chimeSequence == ChimeSequences.CircleSolve || chimeSequence == ChimeSequences.SquareSolve || chimeSequence == ChimeSequences.TriangleSolve){
            SolveHit.SetActive(true);
        }
    }
}
