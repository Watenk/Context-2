using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer 
{
    private float lenght;
    private float timer;

    //--------------------------------------------
    
    public Timer(float lenght){
        this.lenght = lenght;
        timer = lenght;
    }
    
    // Only TimerManager uses this update
    public void Update(){
        if (timer > 0){
            timer -= Time.deltaTime;
        }
    }

    public bool IsDone(){
        if (timer <= 0){
            timer = lenght;
            return true;
        }
        return false;
    }

    public void ChangeLenght(float newLenght){
        lenght = newLenght;
    }
}
