using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopingSound 
{
    public GameObject soundGameObject { get; private set; }

    private AK.Wwise.Event stopEvent;

    // References
    private SoundManager soundManager;

    //---------------------------------------

    public LoopingSound(SoundManager soundManager, GameObject soundGameObject, AK.Wwise.Event startEvent, AK.Wwise.Event stopEvent){
        this.soundManager = soundManager;
        this.soundGameObject = soundGameObject;
        this.stopEvent = stopEvent;
        
        startEvent.Post(soundGameObject);
    }

    public void StopSound(){
        stopEvent.Post(soundGameObject);
        soundManager.StopLoopingSound(this);
    }
}
