using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound 
{
    private GameObject soundInstance;
    private AK.Wwise.Event stopEvent;

    //---------------------------------------

    public Sound(GameObject soundInstance, AK.Wwise.Event stopEvent){
        this.soundInstance = soundInstance;
        this.stopEvent = stopEvent;
    }

    public void StopSound(){
    }
}
