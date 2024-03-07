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

    public Sound(SoundManager soundManager, GameObject soundGameObject, AK.Wwise.Event startEvent, float existLenght){
        timerManager = GameManager.GetService<TimerManager>();
        this.soundManager = soundManager;

        startEvent.Post(soundGameObject);
        existTimer = timerManager.AddTimer(existLenght);
    }

    public void OnFixedUpdate(){
        if (existTimer.IsDone()){
            soundManager.StopSound(this);
        }
    }
}
