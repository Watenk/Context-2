using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InputHandler : IUpdateable
{
    public event Action<Vector2> OnPlayerMove;
    public event Action<Chime> OnChime;
    public event Action<ChimeInputs> OnChimeUp;
    public event Action<ChimeInputs> OnChimeDown;

    private Dictionary<string, Timer> chimeTimers = new Dictionary<string, Timer>();
    private Dictionary<string, bool> chimeBools = new Dictionary<string, bool>();

    // References
    private TimerManager timerManager;

    //---------------------------------------------------------

    public InputHandler(){
        timerManager = GameManager.GetService<TimerManager>();

        chimeTimers.Add("square", timerManager.AddLoopingTimer(ChimeSettings.Instance.LongChimeLenght));
        chimeTimers.Add("triangle", timerManager.AddLoopingTimer(ChimeSettings.Instance.LongChimeLenght));
        chimeTimers.Add("circle", timerManager.AddLoopingTimer(ChimeSettings.Instance.LongChimeLenght));
        chimeTimers.Add("global", timerManager.AddLoopingTimer(ChimeSettings.Instance.LongChimeLenght));

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
            IfChimeDown(button);
        }
        else if (Input.GetAxis(button) == 0 && state && OnChime != null){
            IfChimeUp(button);
        }
    }

    private void IfChimeDown(string button){
        ChimeInputs chimeInput = ConvertButtonToChimeInput(button);
        chimeBools[button] = true;
        chimeTimers[button].Reset();
        OnChimeDown(chimeInput);
    }

    private void IfChimeUp(string button){
        ChimeInputs chimeInput = ConvertButtonToChimeInput(button);
        if (chimeTimers[button].IsDone()){
            OnChime(new Chime(chimeInput, true));
        }
        else{
            OnChime(new Chime(chimeInput, false));
        }
        chimeBools[button] = false;
        OnChimeUp(chimeInput);
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
