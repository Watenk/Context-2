using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerManager : IUpdateable
{
    private List<Timer> timers = new List<Timer>();
    private List<Stopwatch> stopwatches = new List<Stopwatch>();

    //-----------------------------------------------------

    public void OnUpdate(){
        foreach (Timer timer in timers){
            timer.Update();
        }
    }

    public Timer AddLoopingTimer(float lenght){
        Timer newTimer = new Timer(lenght);
        timers.Add(newTimer);
        return newTimer;
    }

    public void RemoveLoopingTimer(Timer timer){
        timers.Remove(timer);
    }

    public Timer AddTimer(float lenght){
        Timer newTimer = new Timer(lenght);
        timers.Add(newTimer);
        return newTimer;
    }

    public void RemoveTimer(Timer timer){
        timers.Remove(timer);
    }

    public Stopwatch AddStopwatch(){
        Stopwatch newStopwatch = new Stopwatch();
        stopwatches.Add(newStopwatch);
        return newStopwatch;
    }

    public void RemoveStopwatch(Stopwatch stopwatch){
        stopwatches.Remove(stopwatch);
    }
}
