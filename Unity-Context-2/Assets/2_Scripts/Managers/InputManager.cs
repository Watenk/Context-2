using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InputManager : IUpdateable
{
    public event Action<Vector2> OnPlayerMove;
    public event Action<Chime> OnChime;

    private Dictionary<string, Timer> chimeTimers = new Dictionary<string, Timer>();
    private Dictionary<string, bool> chimeBools = new Dictionary<string, bool>();

    // References
    private TimerManager timerManager;

    //---------------------------------------------------------

    public InputManager(){
        timerManager = GameManager.GetService<TimerManager>();

        chimeTimers.Add("square", timerManager.AddTimer(ChimeSettings.Instance.LongChimeTime));
        chimeTimers.Add("triangle", timerManager.AddTimer(ChimeSettings.Instance.LongChimeTime));
        chimeTimers.Add("circle", timerManager.AddTimer(ChimeSettings.Instance.LongChimeTime));
        chimeTimers.Add("global", timerManager.AddTimer(ChimeSettings.Instance.LongChimeTime));

        chimeBools.Add("square", false);
        chimeBools.Add("triangle", false);
        chimeBools.Add("circle", false);
        chimeBools.Add("global", false);
    }

    public void OnUpdate(){
        OnPlayerMove(new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")));
        CheckChime("global");
        CheckChime("square");
        CheckChime("triangle");
        CheckChime("circle");
    }

    //---------------------------------------------------------

    private void CheckChime(string button){
        chimeBools.TryGetValue(button, out bool state);
        if (Input.GetAxis(button) > 0 && !state && OnChime != null){
            OnChimeDown(button);
        }
        else if (Input.GetAxis(button) == 0 && state && OnChime != null){
            OnChimeUp(button);
        }
    }

    private void OnChimeDown(string button){
        chimeBools[button] = true;
        chimeTimers[button].ResetTime();
    }

    private void OnChimeUp(string button){
        if (chimeTimers[button].IsDone()){
            OnChime(new Chime(ConvertButtonToChimeInput(button), true));
        }
        else{
            OnChime(new Chime(ConvertButtonToChimeInput(button), false));
        }
        chimeBools[button] = false;
    }

    private ChimeInputs ConvertButtonToChimeInput(string button){

        switch (button){
            case "global":
                return ChimeInputs.global;
            case "square":
                return ChimeInputs.square;
            case "triangle":
                return ChimeInputs.triangle;
            case "circle": 
                return ChimeInputs.circle;
        }

        #if UNITY_EDITOR
            Debug.LogWarning("ConvertButtonToChimeInput failed");
        #endif

        return ChimeInputs.global;
    }
}
