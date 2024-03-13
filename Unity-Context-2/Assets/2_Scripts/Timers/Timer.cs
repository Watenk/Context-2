using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer 
{
    protected float timer;
    protected float lenght;

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

    public virtual bool IsDone(){
        if (timer <= 0){
            return true;
        }
        return false;
    }

    public void ChangeLenght(float newLenght){
        lenght = newLenght;
    }

    public void ChangeCurrentTime(float newTimerTime){
        timer = newTimerTime;
    }

    public void Interrupt(){
        timer = lenght;
    }
}
