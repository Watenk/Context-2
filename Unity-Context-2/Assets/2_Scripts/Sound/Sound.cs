using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound : IFixedUpdateable
{
    public GameObject SoundGameObject { get; private set; }

    private Timer existTimer;

    // References
    private TimerManager timerManager;
    private SoundManager soundManager;

    //------------------------------------------------

    public Sound(SoundManager soundManager, GameObject soundGameObject, AK.Wwise.Event startEvent){
        timerManager = GameManager.GetService<TimerManager>();
        this.soundManager = soundManager;
        SoundGameObject = soundGameObject;

        startEvent.Post(soundGameObject);
        existTimer = timerManager.AddTimer(SoundSettings.Instance.SoundDestroyTime);
    }

    public void OnFixedUpdate(){
        if (existTimer.IsDone()){
            soundManager.StopSound(this);
        }
    }
}
