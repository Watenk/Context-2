using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stopwatch 
{
    private bool runnning;
    private float passedTime;

    //---------------------------------------

    // Only TimerManager uses this update
    public void Update(){
        if (runnning){
            passedTime += Time.deltaTime;
        }
    }

    public void Start(){
        runnning = true;
    }

    public float Stop(){
        runnning = false;
        return passedTime;
    }

    public void Reset(){
        passedTime = 0;
    }
}
