using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopingTimer : Timer
{
    public LoopingTimer(float lenght) : base(lenght){}

    public override bool IsDone(){
        if (timer <= 0){
            timer = lenght;
            return true;
        }
        return false;
    }
}
